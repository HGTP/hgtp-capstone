/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Telecom;

namespace RevisedGestrApp.Droid.Services.PhoneCalls
{
    public class TelecomManagerWrapper : ITelecomManager
    {
        private TelecomManager telecomManager;

        public TelecomManagerWrapper(TelecomManager telecomManager)
        {
            this.telecomManager = telecomManager;
        }

        public void AcceptRingingCall()
        {
            this.telecomManager.AcceptRingingCall();
        }

        public void DeclineRingingCall()
        {
            this.telecomManager.EndCall();
        }
    }
}
