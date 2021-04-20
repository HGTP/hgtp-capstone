using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SendSignal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Sends message via the messaging center. These messages will be received by the AppDelegate in iOS
        /// and MainActivity in android
        /// reference: https://www.youtube.com/watch?v=Z1YzyreS4-o&feature=emb_logo
        /// </summary>
        private void onSendSignal(object sender, EventArgs args)
        {
            //create message sending this as the object and "SendSignal" as the keyword
            MessagingCenter.Send((MainPage)this, "SendSignal");
        }
    }
}
