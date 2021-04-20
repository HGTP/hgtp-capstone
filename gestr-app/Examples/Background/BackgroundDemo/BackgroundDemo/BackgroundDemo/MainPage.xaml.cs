using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BackgroundDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CameBack();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void onStartListening(object sender, EventArgs args)
        {
            //ask the iOS app delegate and android MainActivity.cs to start listening
            MessagingCenter.Send((MainPage)this, "StartListening");
            
            //change label text to alert user that the app is listening for a signal
            Device.BeginInvokeOnMainThread(() =>
            {
                updates.Text = "listening";
            });

        }

        public void CameBack()
        {
            MessagingCenter.Subscribe<ReceivedSignalMessage>(this, "Received Signal", message =>
           {
               Device.BeginInvokeOnMainThread(() =>
               {
                
                    updates.Text = message.getMessage();
                
               });
           });

        
        }

    }
}
