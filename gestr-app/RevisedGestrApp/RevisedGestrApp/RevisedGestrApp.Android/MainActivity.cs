/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android;
using Android.Support.V4.App;
using RevisedGestrApp.Util;
using RevisedGestrApp.Droid.Services;
using RevisedGestrApp.ViewModels;
using RevisedGestrApp.Models;
using RevisedGestrApp.Droid.Services.PhoneCalls;
using RevisedGestrApp.Droid.Services.Texts;
using Android.Telephony;
using RevisedGestrApp.Droid.Services.Music;
using RevisedGestrApp.Droid.Services.DoNotDisturb;
using RevisedGestrApp.Droid.Services.GPS;
using Okta.Xamarin;
using Okta.Xamarin.Android;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Xamarin.Essentials;
using System;

namespace RevisedGestrApp.Droid
{
    [Activity(Label = "Gestr", Icon = "@drawable/gestr_logo_circle_bigger", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : OktaMainActivity<OktaApp>
    {
        internal static MainActivity Instance { get; private set; }
        public bool PhoneRinging { get; private set; }
        private ActionHandler actionHandler;
        private NotificationManager notificationManager;
        private Android.Media.AudioManager audioManager;

        /// <summary>
        /// Steps required in the creation of the mobile app.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("INSERT_APP_CENTER_API_KEY",
                      typeof(Analytics), typeof(Crashes));

            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            PhoneRinging = false;
            

            this.notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            audioManager = (Android.Media.AudioManager)GetSystemService(Context.AudioService);
            actionHandler = new ActionHandler(
                new AudioManager(audioManager),
                new CallManager(),
                new GPSManager(),
                new NotificationManagerWrapper(notificationManager),
                new SmsManagerWrapper(SmsManager.Default));
            StartListening();


            IOktaStateManager state = await OktaContext.Current.SignInAsync();
        }

        /// <summary>
        /// Runs after permissions have been requested. If permissions granted certain features will be enabled. E.g. accepting phone calls
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //Check phone call permissions
            if (requestCode == 123 && grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            {
                Intent serviceStart = new Intent(this, typeof(PhoneCallService));
                this.StartService(serviceStart);
            }
        }

        /// <summary>
        /// Subsrctiption for message that can be customized.
        /// </summary>
        /// <param name="gesture"></param>
        private void SubscribeGeneralMessage(string gesture)
        {
            MessagingCenter.Subscribe<GestureReceivedMessage>(this, gesture, async message =>
            {
                string action = await GestureHandler.GetAction(gesture);
                NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                actionHandler.PerformAction(action);
            });
        }


        /// <summary>
        /// Begins the listening for messages, and signals from the signal app
        /// </summary>
        public void StartListening()
        {
            SubscribeGeneralMessage(Gestures.UP);
            SubscribeGeneralMessage(Gestures.DOWN);
            SubscribeGeneralMessage(Gestures.LEFT);
            SubscribeGeneralMessage(Gestures.RIGHT);
            IMusicManager musicManager = new SpotifyManager();
            actionHandler.RegisterMusicManager(musicManager);
            MessagingCenter.Subscribe<SettingsViewModel>(this, Gestures.SPOTIFY, async message =>
            {
                if (!actionHandler.HasAuthenticatedMusicManager)
                {
                    
                    MessagingCenter.Subscribe<SpotifyLoginMessage>(this, "LoginSuccess", message =>
                    {
                        if (!actionHandler.HasAuthenticatedMusicManager)
                        {
                            actionHandler.UpdateMusicManagerAuthentication(true);
                            MessagingCenter.Send(new SpotifyLoginMessage("RegistrationSuccess", message.HasPremium), "RegistrationSuccess");
                        }
                        
                    });
                    await musicManager.Init();
                }
            });
            MessagingCenter.Subscribe<DNDPermissionMessage>(this, "DNDAdded", message =>
            {
                if (!notificationManager.IsNotificationPolicyAccessGranted)
                {
                    Intent intent = new Intent(Android.Provider.Settings.ActionNotificationPolicyAccessSettings);
                    StartActivity(intent);
                }
                else 
                {
                    MessagingCenter.Send(new DNDPermissionMessage(), "DNDGranted");
                }
            });

            MessagingCenter.Subscribe<string>(this, "GPSRoute", async message =>
            {
                GPSManager gpsManager = new GPSManager();
                GPSHandler handler = new GPSHandler();
                GPSSetting setting = await handler.GetSetting();
                Rootobject root = await gpsManager.GetDirectionsAsync(setting.Destination, setting.Mode);
                if (root.status.Equals("NOT_FOUND"))
                {
                    MessagingCenter.Send<string>("", "Unsuccessful");
                    return;
                }
                MessagingCenter.Send<string, Rootobject>("", "GetRoute", root);

            });
        }

        /// <summary>
        /// Events to start when the siign in with Okta have been completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="signInEventArgs"></param>
        public override void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
        {
            SignInHandler();
        }

        /// <summary>
        /// Handles the login information and prompts for permissions to the phone.
        /// </summary>
        private async void SignInHandler()
        {
            try
            {
                await SecureStorage.SetAsync("access_token", OktaContext.Current.GetToken(TokenKind.AccessToken));
            }
            catch (Exception ex)
            {
                // TODO: Possible that device doesn't support secure storage if this is reached.
                //       Currently undefined plan to deal with this.
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(OktaContext.Current.GetToken(TokenKind.IdToken));
            var claims = jsonToken?.Payload?.Claims;

            var name = claims?.FirstOrDefault(x => x.Type == "name")?.Value;
            var preferredUsername = claims?
                .FirstOrDefault(x => x.Type == "preferred_username")?.Value;

            string userName = name.ToString();
            string preferredName = preferredUsername.ToString();
            LoginViewModel login = new LoginViewModel(userName,preferredName);
            //ask for permissions
            var permissions = new string[]
            {
                Manifest.Permission.CallPhone,
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.AccessNotificationPolicy,
                Manifest.Permission.AnswerPhoneCalls,
                Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin,
                Manifest.Permission.Internet,
                Manifest.Permission.ReadContacts,
                Manifest.Permission.ReadPhoneState,
                Manifest.Permission.SendSms,
                Manifest.Permission.ReadContacts,
                Manifest.Permission.ReadSms,
            };
            ActivityCompat.RequestPermissions(this, permissions, 123);
            OktaContext.Current.SignOutStarted += SignOutHandler;
            await Shell.Current.GoToAsync("//LoginPage");

        }

        /// <summary>
        /// Removes Spotify data upon log out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SignOutHandler(object sender, SignOutEventArgs e)
        {
            actionHandler.RegisterMusicManager(new SpotifyManager());
            await Shell.Current.GoToAsync("//LoadingPage");
        }
    }
}




