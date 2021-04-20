/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Microsoft.AppCenter.Analytics;
using RevisedGestrApp.Models;
using System;
using System.Collections.Generic;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// This handles interaction with App Analytics.
    /// </summary>
    class GestrAnalytics : IAnalytics
    {
        /// <summary>
        /// Sends a setting event to App Analytics.
        /// </summary>
        /// <param name="setting"></param>
        public void TrackSettingUpdate(Setting setting)
        {
            Analytics.TrackEvent("Gesture Setting", new Dictionary<string, string>{
                {"Event", "Updated Action"  },
                {"User", "<UserName>"},
                {"Date", DateTime.Now.ToString("MM/dd/yyyy HH:mm tt") },
                {"Gesture", setting.Gesture},
                {"Action", setting.PhoneAction }
            });
        }
    }
}
