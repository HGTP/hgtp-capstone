/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RevisedGestrApp.Services;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RevisedGestrApp.Models;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.Droid.Services.GPS
{
    public class GPSHandler
    {
        public GPSHandler()
        {
        }

        /// <summary>
        /// Gets default GPS setting data if it is there
        /// </summary>
        /// <returns>GPSSetting or null</returns>
        public  async Task<GPSSetting> GetSetting()
        {
            var settingsStore = new GPSSettingStore();
            GPSSetting setting = await settingsStore.GetAsync();
            return setting;
        }

    }
}