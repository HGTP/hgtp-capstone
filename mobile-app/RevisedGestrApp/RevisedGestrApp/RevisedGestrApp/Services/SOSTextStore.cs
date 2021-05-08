/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// This represents a text message specifically for an emergency contact person.
    /// </summary>
    public class SOSTextStore : IDataStore<Text>
    {
        /// <summary>
        /// Adds a new emergency text message.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddAsync(Text data)
        {
            Application.Current.Properties["SOS_text"] = data.TextMsg;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Deletes the current emergency text message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task DeleteAsync(string msg)
        {
            return Task.Run(() =>
            {
                Application.Current.Properties["SOS_text"] = null;
            });
        }

        /// <summary>
        /// Gets the current emergency text message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task<Text> GetAsync(string msg)
        {
            return Task.Run(() =>
            {
                var message = new Text
                {
                    TextMsg = (string)Application.Current.Properties["SOS_text"]
                };
                return message;
            });
        }

        /// <summary>
        /// Updates the current emergency text message.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Text data)
        {
            Application.Current.Properties["SOS_text"] = data.TextMsg;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
