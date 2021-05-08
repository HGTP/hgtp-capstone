/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// This represents a text message specifically for the general contact persons.
    /// </summary>
    public class TextStore : IDataStore<Text>
    {
        /// <summary>
        /// Adds a new text message.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddAsync(Text data)
        {
            Application.Current.Properties["text"] = data.TextMsg;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Deletes the current text message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task DeleteAsync(string msg)
        {
            return Task.Run(() =>
            {
                Application.Current.Properties["text"] = null;
            });
        }

        /// <summary>
        /// Gets the current text message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task<Text> GetAsync(string msg)
        {
            return Task.Run(() =>
            {
                var message = new Text
                {
                    TextMsg = (string)Application.Current.Properties["text"]
                };
                return message;
            });
        }

        /// <summary>
        /// Updates the current text message.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Text data)
        {
            Application.Current.Properties["text"] = data.TextMsg;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
