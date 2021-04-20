/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// Represents the system context.
    /// </summary>
    public interface IContext
    {
        object GetSystemService(string service);
    }
}
