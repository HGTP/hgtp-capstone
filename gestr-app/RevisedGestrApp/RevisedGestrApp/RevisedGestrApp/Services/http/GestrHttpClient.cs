/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using GestrMobile.Services.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GestrMobile.Services.http
{
    /// <summary>
    /// A wrapper class to do http requests specific to our product.
    /// </summary>
    public class GestrHttpClient : IHttpClient
    {
        private static HttpClient client;

        public GestrHttpClient()
        {
            if (client == null)
            {
                client = new HttpClient();
                // https://docs.microsoft.com/en-us/xamarin/cross-platform/deploy-test/connect-to-local-web-services
                //client.BaseAddress = new Uri("http://10.0.2.2:8080/");
                client.BaseAddress = new Uri("https://hgtp-api.azurewebsites.net/");
            }
        }

        /// <summary>
        /// Updates the access token used in requests.
        /// </summary>
        /// <returns></returns>
        private async Task<string> SetAccessToken()
        {
            string accessToken;
            try
            {
                accessToken = await SecureStorage.GetAsync("access_token");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.

                // Unsafe storage if device doesn't support secure storage...
                // Application.Current.Properties ["access_token"];
                accessToken = null;
            }
            return accessToken;
        }

        /// <summary>
        /// This is required per the interface but not used in our product at the moment.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Delete(string requestUri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a get request.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> Get(string requestUri)
        {
            await SetAccessToken();
            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// This is required by the interface but not used in our product at the moment.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Post(string requestUri, HttpContent content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a put request.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Put(string requestUri, HttpContent content)
        {
            await SetAccessToken();
            try
            {
                HttpResponseMessage response = await client.PutAsync(requestUri, content);
                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
