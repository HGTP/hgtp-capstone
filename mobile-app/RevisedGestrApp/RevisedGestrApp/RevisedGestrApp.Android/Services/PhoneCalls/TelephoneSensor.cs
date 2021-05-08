/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Content;
using Android.Telephony;
using Xamarin.Forms;

/// <summary>
/// Code by Andreas Kutner Andreas Kuntner "Reacting to incoming phone calls in Xamarin.Android #1"
/// https://www.mobilemotion.eu/?p=2442
/// posted July 13, 2019
/// Edited by Misha Griego Nov. 2020
/// </summary>
namespace RevisedGestrApp.Droid.Services.PhoneCalls
{

    [BroadcastReceiver]
    public class TelephoneSensor : PhoneStateListener
    {

        /// <summary>
        ///Senses changes in the the Phone state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="incomingNumber"></param>
        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            //if a call is coming
            if (state == CallState.Ringing)
            {
                MessagingCenter.Send(this, "Phone Call");
            }
        }
    }
}

