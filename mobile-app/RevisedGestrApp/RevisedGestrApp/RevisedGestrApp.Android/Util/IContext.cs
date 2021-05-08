/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Droid.Services.PhoneCalls;

namespace RevisedGestrApp.Droid.Util
{
    public interface IContext
    {
        ITelecomManager GetSystemService();
    }
}
