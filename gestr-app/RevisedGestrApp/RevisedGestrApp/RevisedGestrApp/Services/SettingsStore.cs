/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GestrMobile.Services.http;
using GestrMobile.Services.Http;
using RevisedGestrApp.Models;
using Xamarin.Forms;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// A store where you can manage settings for gestures and actions.
    /// </summary>
    public class SettingsStore : IDataStore<Setting>
    {
        private IHttpClient client;
        public SettingsStore(IHttpClient client) => this.client = client;
        public SettingsStore() => this.client = new GestrHttpClient();

        public async Task AddAsync(Setting data)
        {
            await client.Put(
                $"/preset/Music/{data.Gesture}", 
                new StringContent(
                    "{\"phoneAction\":\"" + data.PhoneAction + "\"}",
                    Encoding.UTF8,
                    "application/json"
                )
            );
        }

        public Task DeleteAsync(string gesture)
        {
            return Task.Run(() =>
            {
                Application.Current.Properties[gesture] = null;
            });
        }

        public Task<Setting> GetAsync(string gesture)
        {
            return Task.Run(async () =>
            {
                var response = await client.Get($"/preset/Music/{gesture}");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var setting = JsonSerializer.Deserialize<Setting>(response, options);
                return setting;
            });
        }

        public async Task UpdateAsync(Setting data)
        {
            await client.Put(
                $"/preset/Music/{data.Gesture}",
                new StringContent(
                    "{\"phoneAction\":\"" + data.PhoneAction + "\"}",
                    Encoding.UTF8,
                    "application/json"
                )
            );
        }
    }
}

