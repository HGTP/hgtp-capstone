/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using RevisedGestrApp.Droid.Util;

/// <summary>
/// Based off code by Krishna Sharma on StackOverflow https://stackoverflow.com/questions/51871673/how-to-answer-automatically-and-programatically-an-incoming-call-on-android
/// Edited by Misha Griego November 2020 
///</summary>
namespace RevisedGestrApp.Droid.Services.PhoneCalls.Deprecated
{
    /// <summary>
    /// A class that receives a context and can receive the phone call.
    /// For Android OS 8.0 Oreo and above
    /// </summary>
    public class CallManager
    {
        private IContext context;
        private IVersionControl versionControl;

        //give the context where the phone call is happening
        public CallManager(IContext context, IVersionControl versionControl)
        {
            this.context = context;
            this.versionControl = versionControl;
        }

        /// <summary>
        /// Accepts a phone call
        /// </summary>
        public void AcceptCall()
        {
            try
            {
                if (versionControl.BuildVersionIsValid())
                {
                    //Use telecom manager to get the service
                    ITelecomManager telecomManager = context.GetSystemService();
                    if (telecomManager != null)
                    {
                        //accept call
                        telecomManager.AcceptRingingCall();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: " + e.Message);
            }
        }

        /// <summary>
        /// Declines a phone call
        /// </summary>
        public void DeclineCall()
        {
            try
            {
                if (versionControl.BuildVersionIsValid())
                {
                    //Use telecom manager to get the service
                    ITelecomManager telecomManager = context.GetSystemService();
                    if (telecomManager != null)
                    {
                        //decline call
                        telecomManager.DeclineRingingCall();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: " + e.Message);
            }
        }
    }
}
