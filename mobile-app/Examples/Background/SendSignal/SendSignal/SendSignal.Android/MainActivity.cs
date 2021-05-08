using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;

namespace SendSignal.Droid
{
    [Activity(Label = "SendSignal", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            ListenForMessage();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
        }

        /// <summary>
        /// Listens for messages from Message center in Xamarin
        /// reference sending broadcast: https://developer.android.com/guide/components/broadcasts#java
        /// reference messaging center: https://www.youtube.com/watch?v=Z1YzyreS4-o&feature=emb_logo
        /// </summary>
        private void ListenForMessage()
        {
            //listen for message with Main Page object and "SendSignal" keyword
            MessagingCenter.Subscribe<MainPage>(this, "SendSignal", message =>
            {
                //create inent with action and data
                Intent intent = new Intent();
                intent.SetAction("Accept Call");
                intent.PutExtra("data", "Accept Call");
                
                //send the intent
                SendBroadcast(intent);
                
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}