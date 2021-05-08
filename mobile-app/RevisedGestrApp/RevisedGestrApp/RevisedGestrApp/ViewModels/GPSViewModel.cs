/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.ViewModels
{
    public class GPSViewModel : BaseViewModel
    {
       
        private readonly GPSSettingStore gpsSettingStore;

        /// <summary>
        /// The currently selected mode
        /// </summary>
        public string SelectedMode
        {
            get;
            set;
        }

    
        /// <summary>
        /// The most recently save destination
        /// </summary>
        public string SavedDest
        {
            get;
            set;
        }
        /// <summary>
        /// The most recently saved mode
        /// </summary>
        public string SavedMode
        {
            get;
            set;
        }

        /// <summary>
        /// The current Destination text
        /// </summary>
        public string GPSDestination
        {
            get;
            set;
        }

      /// <summary>
      /// Creates a gpsSettingStore and runs InitSettings
      /// </summary>
        public GPSViewModel()
        {
            Title = "GPS";

            this.gpsSettingStore = gpsSettingStore ?? new GPSSettingStore();
            SavedDest = "";
            SavedMode = "bicycling";
            Task.Run(InitSettingsAsync).Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task InitSettingsAsync()
        {
            GPSSetting setting;
            try
            {
                setting = await gpsSettingStore.GetAsync();
            
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                setting= new GPSSetting(){Destination = "",Mode = ""};
          
            }
            SavedMode = setting.Mode;
            SavedDest = setting.Destination;
            SelectedMode = setting.Mode;
            GPSDestination = setting.Destination;
        }

      

        /// <summary>
        /// 
        /// </summary>
        public async Task UpdateSettingAsync()
        {
            GPSSetting setting = new GPSSetting();
            setting.Mode = SelectedMode;
            setting.Destination = GPSDestination;
            await gpsSettingStore.UpdateAsync(setting);
            SavedDest = setting.Destination;
            SavedMode = setting.Mode;
        }

    }
}

