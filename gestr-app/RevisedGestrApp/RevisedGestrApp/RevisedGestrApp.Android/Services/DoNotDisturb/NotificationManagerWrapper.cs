/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RevisedGestrApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RevisedGestrApp.Droid.Services.DoNotDisturb
{
    public class NotificationManagerWrapper : INotificationManager
    {
        private NotificationManager notificationManager;

        /// <summary>
        /// Constructor for the NotificationManagerWrapper
        /// </summary>
        /// <param name="notificationManager">NotificationManager that will back the functionality of the NotificationManagerWrapper</param>
        public NotificationManagerWrapper(NotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

        /// <summary>
        /// Toggles do not disturb if the app has been granted that permission. Notifies the app if the permission has not been granted.
        /// </summary>
        /// <returns>true if DND was toggled, false if DND permission has not been granted</returns>
        public bool ToggleDoNotDisturb()
        {
            if (notificationManager.IsNotificationPolicyAccessGranted)
            {
                InterruptionFilter filter = notificationManager.CurrentInterruptionFilter;
                if (filter.Equals(InterruptionFilter.None))
                {
                    notificationManager.SetInterruptionFilter(InterruptionFilter.All);
                }
                else
                {
                    notificationManager.SetInterruptionFilter(InterruptionFilter.None);
                }
                MessagingCenter.Send(new DNDPermissionMessage(), "DNDGranted");
                return true;
            }
            else
            {
                MessagingCenter.Send(new DNDPermissionMessage(), "DNDNotGranted");
                return false;
            }
        }
    }
}