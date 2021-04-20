using System;
using System.Collections.Generic;
using System.Text;

namespace RevisedGestrApp.Util
{
   
    public class SpotifyLoginMessage
    {
        public string LoginStatus { get; set; }
        
        public bool HasPremium { get; }
        public SpotifyLoginMessage(string loginStatus, bool hasPremium)
        {
            LoginStatus = loginStatus;
            HasPremium = hasPremium;
        }
    
    }
}
