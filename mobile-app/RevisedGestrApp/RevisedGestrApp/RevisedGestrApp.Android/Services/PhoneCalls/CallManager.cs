/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Content;
using System;
using Xamarin.Forms;

namespace RevisedGestrApp.Droid.Services.PhoneCalls
{
    public class CallManager : ICallManager
    {
        public void Dial(string phoneNumber)
        {
            try
            {
                /// Based on Yksh's Stackoverflow response: https://stackoverflow.com/a/33889130
                /// 03/29/2021
                var uri = Android.Net.Uri.Parse(string.Format("tel:{0}", phoneNumber));
                var intent = new Intent(Intent.ActionCall, uri);
                Forms.Context.StartActivity(intent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}