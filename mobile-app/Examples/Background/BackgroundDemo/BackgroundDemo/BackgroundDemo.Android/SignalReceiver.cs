using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace BackgroundDemo.Droid
{
    [BroadcastReceiver]
    public class SignalReceiver : BroadcastReceiver
    {
        /// <summary>
        /// Specifies what to do when the Broadcast is received
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            
            //create a message for the data
            ReceivedSignalMessage message = new ReceivedSignalMessage("Received data:" + intent.GetStringExtra("data"));
            
            //send object with message data in it and the keywords
            MessagingCenter.Send<ReceivedSignalMessage>(message, "Received Signal");

        }
    }
}