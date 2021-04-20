/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;

/// <summary>
/// Code by Andreas Kutner Andreas Kuntner "Reacting to incoming phone calls in Xamarin.Android #1"
/// https://www.mobilemotion.eu/?p=2442
/// posted July 13, 2019
/// Edited by Misha Griego Nov. 2020
/// </summary>
namespace RevisedGestrApp.Droid.Services.PhoneCalls
{
    [Service]
    public class PhoneCallService : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new TelephoneSensor in order to listen to the phone states
        /// </summary>
        /// <param name="intent">the intent coming in</param>
        /// <param name="flags">flag associated with the intent</param>
        /// <param name="startId">The id for the start command</param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            var callDetector = new TelephoneSensor();
            var tm = (TelephonyManager)base.GetSystemService(TelephonyService);
            tm.Listen(callDetector, PhoneStateListenerFlags.CallState);

            return StartCommandResult.Sticky;
        }
    }
}
