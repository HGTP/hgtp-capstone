using RevisedGestrApp.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.Services
{
    public class GPSSettingStore : IDataStore<GPSSetting>
    {

        /// <summary>
        /// Stores default GPSSetting data in the phone
        /// Note: this may overwrite any data if there already is some
        /// </summary>
        /// <param name="data">GPS setting data to store</param>
        /// <returns></returns>
        public async Task AddAsync(GPSSetting data)
        {
            Application.Current.Properties["GPSDestination"] = data.Destination;
            Application.Current.Properties["TransportationMode"] = data.Mode;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Stores GPSSetting data in the phone for id
        /// Note: this may overwrite any data if there already is some 
        /// </summary>
        /// <param name="data">GPSSetting with setting data</param>
        /// <param name="id">id of the specific data</param>
        /// <returns></returns>
        public async Task AddAsync(GPSSetting data, string id)
        {
            Application.Current.Properties[id+"_Destination"] = data.Destination;
            Application.Current.Properties[id+"_Mode"] = data.Mode;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Deletes specific GPS settings from storage in the phone
        /// for id to null
        /// </summary>
        /// <param name="id">specifies the id of the specific GPS settings</param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            Application.Current.Properties[id + "_Destination"] = null;
            Application.Current.Properties[id + "_Mode"] = null;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// sets default GPS settings from storage in the phone to null
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            Application.Current.Properties["GPSDestination"] = null;
            Application.Current.Properties["TransportationMode"] = null;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Gets specific GPSSetting based on id
        /// </summary>
        /// <param name="id">id for the specific GPSSetting</param>
        /// <returns>GPSSetting with setting data for id</returns>
        public Task<GPSSetting> GetAsync(string id)
        {
            return Task.Run(() =>
            {
                var setting = new GPSSetting
                {
                    Destination = (string)Application.Current.Properties[id+"_Destination"],
                    Mode = (string)Application.Current.Properties[id+"_Mode"],
                };
                return setting;
            });
        }

        /// <summary>
        /// Gets default GPS settings
        /// </summary>
        /// <returns>GPSSetting containing default GPS settings</returns>
        public Task<GPSSetting> GetAsync()
        {
            return Task.Run(() =>
            {
                var setting = new GPSSetting
                {
                    Destination = (string)Application.Current.Properties["GPSDestination"],
                    Mode = (string)Application.Current.Properties["TransportationMode"],
                };
                return setting;
            });
        }

        /// <summary>
        /// Updates a specific GPSSetting specified by the id
        /// </summary>
        /// <param name="data">GPSSetting with setting data</param>
        /// <param name="id">id of the data to update</param>
        /// <returns></returns>
        public async Task UpdateAsync(GPSSetting data, string id)
        {
            Application.Current.Properties[id+"_Destination"] = data.Destination;
            Application.Current.Properties[id+"_Mode"] = data.Mode;
            await Application.Current.SavePropertiesAsync();
        }


        /// <summary>
        /// Updates Default GPS data
        /// </summary>
        /// <param name="data">GPSSetting with setting data</param>
        /// <returns></returns>
        public async Task UpdateAsync(GPSSetting data)
        {
            Application.Current.Properties["GPSDestination"] = data.Destination;
            Application.Current.Properties["TransportationMode"] = data.Mode;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
