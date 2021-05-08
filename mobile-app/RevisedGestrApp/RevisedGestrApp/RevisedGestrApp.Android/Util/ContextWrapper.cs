/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Content;
using Android.Telecom;
using RevisedGestrApp.Droid.Services.PhoneCalls;

namespace RevisedGestrApp.Droid.Util
{
    public class ContextWrapper : IContext
    {
        Context context;

        public ContextWrapper(Context context)
        {
            this.context = context;
        }

        public ITelecomManager GetSystemService()
        {
            var telecomService = Context.TelecomService;
            var telecomManager = (TelecomManager)context.GetSystemService(telecomService);
            return new TelecomManagerWrapper(telecomManager);
        }
    }
}
