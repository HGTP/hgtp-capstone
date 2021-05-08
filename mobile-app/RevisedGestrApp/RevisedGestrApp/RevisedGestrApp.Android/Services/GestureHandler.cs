/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Services;
using RevisedGestrApp.Models;
using System.Threading.Tasks;

namespace RevisedGestrApp.Droid.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class Gestures
    {
        public const string UP = "UP";
        public const string DOWN = "DOWN";
        public const string LEFT = "LEFT";
        public const string RIGHT = "RIGHT";
        public const string SPOTIFY = "SPOTIFY";
    }

    /// <summary>
    /// This is the go to place for knowing the user's settings for which 
    /// action matches which gesture and which gesture matches which action.
    /// </summary>
    public static class GestureHandler
    {

        public static async Task<string> GetAction(string gesture)
        {
            var settingsStore = new SettingsStore();
            Setting setting = await settingsStore.GetAsync(gesture);
            return setting.PhoneAction;
        }
    }
}
