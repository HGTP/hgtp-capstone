/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Content;
using Xamarin.Forms;

namespace RevisedGestrApp.Droid.Services.PhoneCalls.Deprecated
{
    [BroadcastReceiver]
    public class SignalReceiver : BroadcastReceiver
    {
        /// <summary>
        /// Specifies what to do when the Broadcast is received. This code is intended to work with the Signal App
        /// to receive signals representing gestures
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override async void OnReceive(Context context, Intent intent)
        {
            // TODO: we need a way to know what gesture the incoming signal represents
            //       if intent is the signal representing the accept call gesture, continue.

            MessagingCenter.Send(this, "A");
        }
    }
}
