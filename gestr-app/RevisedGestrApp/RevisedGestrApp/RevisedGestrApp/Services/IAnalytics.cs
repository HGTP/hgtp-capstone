/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// Represents something that interacts with App Analytics.
    /// </summary>
    public interface IAnalytics
    {
        void TrackSettingUpdate(Setting setting);
    }
}
