/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevisedGestrApp.Droid.Services.Music
{
    public class AudioManager : IAudioManager
    {
        private Android.Media.AudioManager audioManager;

        public AudioManager(Android.Media.AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        public void VolumeDown()
        {
            audioManager.AdjustVolume(Android.Media.Adjust.Lower, Android.Media.VolumeNotificationFlags.AllowRingerModes);

        }

        public void VolumeUp()
        {
            audioManager.AdjustVolume(Android.Media.Adjust.Raise, Android.Media.VolumeNotificationFlags.AllowRingerModes);
        }
    }
}