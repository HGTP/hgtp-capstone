/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.OS;

namespace RevisedGestrApp.Droid.Util
{
    public class VersionControl : IVersionControl
    {
        public bool BuildVersionIsValid()
        {
            // Build is the targeted 8.0 Oreo OS or above
            return Build.VERSION.SdkInt >= BuildVersionCodes.O;
        }
    }
}
