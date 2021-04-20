/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using System.Threading.Tasks;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// Represents a login provider.
    /// </summary>
    public interface ILoginProvider
    {
        Task<AuthInfo> LoginAsync();
    }
}
