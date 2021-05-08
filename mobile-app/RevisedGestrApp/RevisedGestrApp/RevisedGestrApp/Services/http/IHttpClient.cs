/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Net.Http;
using System.Threading.Tasks;

namespace GestrMobile.Services.Http
{
    /// <summary>
    /// Represents a generic http client.
    /// </summary>
    public interface IHttpClient
    {
        Task<string> Get(string requestUri);
        Task<HttpResponseMessage> Post(string requestUri, HttpContent content);
        Task<HttpResponseMessage> Put(string requestUri, HttpContent content);
        Task<HttpResponseMessage> Delete(string requestUri);
    }
}
