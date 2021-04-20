/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;
using Xamarin.Forms;
using RevisedGestrApp.Util;

namespace RevisedGestrApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public bool LoadingSettings { get; private set; } = true;
        public string DNDPermissionStatus { get; private set; } = "";

        public IList<string> Actions
        {
            get
            {
                return new List<string>()
                {
                    "Emergency Dial",
                    "Emergency Text",
                    "Next GPS Direction",
                    "Play",
                    "Pause",
                    "Read Recent Text Aloud",
                    "Send Text",
                    "Skip",
                    "Turn On/Off Do Not Disturb",
                    "Volume Up",
                    "Volume Down"
                };
            }
        }

        public string SpotifyStatus { get; set; }
        private Setting selectedUP = new Setting() { Gesture = Gestures.UP };
        private Setting selectedDOWN = new Setting() { Gesture = Gestures.DOWN };
        private Setting selectedLEFT = new Setting() { Gesture = Gestures.LEFT };
        private Setting selectedRIGHT = new Setting() { Gesture = Gestures.RIGHT };
        private bool spotifyAuthentication = false;
        private bool afterInit = false;
        public string SelectedUP
        {
            get { return GetSelectedAction(ref selectedUP); }
            set
            {
                SetSelectedAction(ref selectedUP, value);
                UpdateSetting(selectedUP);
            }
        }
        public string SelectedDOWN
        {
            get { return GetSelectedAction(ref selectedDOWN); }
            set
            {
                SetSelectedAction(ref selectedDOWN, value);
                UpdateSetting(selectedDOWN);
            }
        }
        public string SelectedLEFT
        {
            get { return GetSelectedAction(ref selectedLEFT); }
            set
            {
                SetSelectedAction(ref selectedLEFT, value);
                UpdateSetting(selectedLEFT);
            }
        }
        public string SelectedRIGHT
        {
            get { return GetSelectedAction(ref selectedRIGHT); }
            set
            {
                SetSelectedAction(ref selectedRIGHT, value);
                UpdateSetting(selectedRIGHT);
            }
        }

        private readonly IDataStore<Setting> settingsDataStore;
        private readonly IAnalytics analytics;

        public SettingsViewModel(IDataStore<Setting> settingsDataStore = null, IAnalytics analytics = null) 
        {
            Title = "Settings";
            SpotifyStatus = "Not Logged in with Spotify.";
            this.settingsDataStore = settingsDataStore ?? new SettingsStore();
            this.analytics = analytics ?? new GestrAnalytics();
            Task initSettings = Task.Run(InitSettings);           
        }

        private async void InitSettings()
        {
            LoadingSettings = true;
            Setting settingUp;
            try
            {
                settingUp = await settingsDataStore.GetAsync(selectedUP.Gesture);
            } 
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                settingUp = new Setting() { Gesture = Gestures.UP };
            }
            SetSelectedAction(ref selectedUP, settingUp.PhoneAction);

            Setting settingDown;
            try
            {
                settingDown = await settingsDataStore.GetAsync(selectedDOWN.Gesture);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                settingDown = new Setting() { Gesture = Gestures.DOWN };
            }
            SetSelectedAction(ref selectedDOWN, settingDown.PhoneAction);

            Setting settingLeft;
            try
            {
                settingLeft = await settingsDataStore.GetAsync(selectedLEFT.Gesture);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                settingLeft = new Setting() { Gesture = Gestures.LEFT };
            }
            SetSelectedAction(ref selectedLEFT, settingLeft.PhoneAction);

            Setting settingRight;
            try
            {
                settingRight = await settingsDataStore.GetAsync(selectedRIGHT.Gesture);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                settingRight = new Setting() { Gesture = Gestures.RIGHT };
            }
            SetSelectedAction(ref selectedRIGHT, settingRight.PhoneAction);
            LoadingSettings = false;
            OnPropertyChanged("LoadingSettings");
            IList<string> initPhoneActions = new List<string>() { selectedDOWN.PhoneAction, selectedUP.PhoneAction, selectedLEFT.PhoneAction, selectedRIGHT.PhoneAction };
            if (initPhoneActions.Contains("Turn On/Off Do Not Disturb"))
            {
                GetDNDPermission();
            }
            IList<string> spotifyControls = new List<string> { "Play", "Pause", "Skip" };
            if (spotifyControls.Contains(selectedUP.PhoneAction) || spotifyControls.Contains(selectedDOWN.PhoneAction) || spotifyControls.Contains(selectedRIGHT.PhoneAction) || spotifyControls.Contains(selectedLEFT.PhoneAction))
            {
                LoginToSpotify();
            }
            afterInit = true;
        }

        private string GetSelectedAction(ref Setting setting)
        {
            return setting.PhoneAction;
        }


        private void SetSelectedAction(ref Setting setting, string newAction)
        {
            if (newAction != setting.PhoneAction)
            {
                setting.PhoneAction = newAction;
                OnPropertyChanged("Selected" + setting.Gesture);
            }
        }

        /// <summary>
        /// Updates the setting in persistant storage.
        /// </summary>
        /// <param name="setting"></param>
        private void UpdateSetting(Setting setting)
        {
            IList<string> spotifyControls = new List<string> { "Play", "Pause", "Skip" };
            if (afterInit && !spotifyAuthentication && spotifyControls.Contains(setting.PhoneAction))
            {
                LoginToSpotify();
            }
            if (afterInit && setting.PhoneAction == "Turn On/Off Do Not Disturb")
            {
                GetDNDPermission();
            }
            settingsDataStore.UpdateAsync(setting);
            analytics.TrackSettingUpdate(setting);          
        }

        /// <summary>
        /// Starts the Spotify log in process.
        /// </summary>
        public void LoginToSpotify()
        {
            MessagingCenter.Subscribe<SpotifyLoginMessage>(this, "RegistrationSuccess", message =>
            {
                if (message.HasPremium)
                {
                    SpotifyStatus = "Logged in with Spotify. Please remember to open the Spotify app and perform an action to bring the focus to your device. ";
                }
                else
                {
                    SpotifyStatus = "Logged in with Spotify, but you do not have a Spotify Premium subscription. Spotify controls will not execute unless the account has a Premium subscription. Try upgrading to Premium or changing your gesture settings. If you upgrade, please remember to open the Spotify app and perform an action to bring the focus to your device. ";
                }
                
                OnPropertyChanged("SpotifyStatus");
                spotifyAuthentication = true;
            });
            lock (Actions)
            {
                if (!spotifyAuthentication)
                {
                    MessagingCenter.Send(this, Gestures.SPOTIFY);
                }
            }
        }

        /// <summary>
        /// Sends a message in the MessagingCenter notifying that a DND action has been added.
        /// </summary>
        public void GetDNDPermission()
        {
            MessagingCenter.Send(new DNDPermissionMessage(), "DNDAdded");
            MessagingCenter.Subscribe<DNDPermissionMessage>(this, "DNDGranted", message =>
            {
                DNDPermissionStatus = "";
                OnPropertyChanged("DNDPermissionStatus");
            });
            MessagingCenter.Subscribe<DNDPermissionMessage>(this, "DNDNotGranted", message =>
            {
                DNDPermissionStatus = "Permission has not been granted to toggle do not disturb. Try granting permission or changing the gesture setting.";
                OnPropertyChanged("DNDPermissionStatus");

                MessagingCenter.Send(new DNDPermissionMessage(), "DNDAdded");
            });
        }
    }
}
