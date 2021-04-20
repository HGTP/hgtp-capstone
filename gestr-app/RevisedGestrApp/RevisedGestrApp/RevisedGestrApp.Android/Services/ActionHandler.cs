/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Droid.Services.DoNotDisturb;
using RevisedGestrApp.Droid.Services.GPS;
using RevisedGestrApp.Droid.Services.Music;
using RevisedGestrApp.Droid.Services.PhoneCalls;
using RevisedGestrApp.Droid.Services.Texts;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RevisedGestrApp.Droid.Services
{
    public class ActionHandler
    {
        private IAudioManager audioManager;
        private ICallManager callManager;
        private IGPSManager gpsManager;
        public bool HasAuthenticatedMusicManager { get; set; }
        private IMusicManager musicManager;
        private INotificationManager notificationManager;
        private ISmsManager smsManager;

        public ActionHandler(
            IAudioManager audioManager = null,
            ICallManager callManager = null,
            IGPSManager gpsManager = null,
            INotificationManager notificationManager = null,
            ISmsManager smsManager = null)
        {
            this.audioManager = audioManager ?? this.audioManager;
            this.callManager = callManager ?? this.callManager;
            this.gpsManager = gpsManager ?? this.gpsManager;
            this.notificationManager = notificationManager ?? this.notificationManager;
            this.smsManager = smsManager ?? this.smsManager;
            this.audioManager = audioManager ?? this.audioManager;
            HasAuthenticatedMusicManager = false;
        }

        public void RegisterMusicManager(IMusicManager musicManager)
        {
            this.musicManager = musicManager;           
        }

        public void UpdateMusicManagerAuthentication (bool authenticatedMusicManager)
        {
            HasAuthenticatedMusicManager = authenticatedMusicManager;
        }


        public bool PhoneRinging { get; private set; }

        public async Task PerformAction(string action, IMusicManager musicManager = null)
        {
            this.musicManager = musicManager ?? this.musicManager;

            if (action.Equals("Emergency Dial"))
            {
                var sosContactStore = new SOSContactStore();
                var emergencyContacts = await sosContactStore.ListAsync();
                // This version uses the first listed emergency contact as the one to call.
                Contact emergencyContact = null;
                foreach (var contact in emergencyContacts)
                {
                    emergencyContact = contact;
                    break;
                }

                if (emergencyContact != null)
                {
                    this.callManager.Dial(emergencyContact.PhoneNumber);
                }
            }
            else if (action.Equals("Next GPS Direction"))
            {
                gpsManager.ReadNextDirectionAsync();

            }
            else if (action.Equals("Play"))
            {
                bool didItWork = await this.musicManager.Resume();
            }
            else if (action.Equals("Pause"))
            {
                bool didItWork = await this.musicManager.Pause();
            }
            else if (action.Equals("Skip"))
            {
                bool didItWork = await this.musicManager.Skip();
            }
            else if (action.Equals("Send Text"))
            {
                ObservableCollection<Contact> contacts = await TextMessageInfoHandler.GetContacts("predeterminedTexts");
                Text textMessage = await TextMessageInfoHandler.GetText("text");
           
                foreach (Contact c in contacts)
                {
                    //TODO: find a way to handle bad phone numbers. View can do basic handling. Notification?
                    string number = c.PhoneNumber;
                    number = number.Replace("-", "");
                    try
                    {
                        smsManager.SendTextMessage(number, textMessage.TextMsg);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            else if (action.Equals("Emergency Text"))
            {
                ObservableCollection<Contact> contacts = await TextMessageInfoHandler.GetContacts("SOS_predeterminedTexts");
                Text textMessage = await TextMessageInfoHandler.GetText("SOS_text");
                bool sendGPS = await TextMessageInfoHandler.GetSendGPS("Send_GPS");

                foreach (Contact c in contacts)
                {
                    string number = c.PhoneNumber;
                    number = number.Replace("-", "");
                    try
                    {
                        if (sendGPS)
                        {
                            string loc = await gpsManager.GetGPSCoordinates();
                            smsManager.SendTextMessage(number, textMessage.TextMsg + "\nCurrent GPS Location:\n" + loc);
                        }
                        else
                            smsManager.SendTextMessage(number, textMessage.TextMsg);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            else if (action.Equals("Read Recent Text Aloud"))
            {
                // Takes care of the entire read sms aloud flow
                smsManager.ReadRecentText();
            }
            else if (action.Equals("Turn On/Off Do Not Disturb"))
            {
                notificationManager.ToggleDoNotDisturb();
            }
            else if (action.Equals("Volume Up"))
            {
                audioManager.VolumeUp();
            }
            else if (action.Equals("Volume Down"))
            {
                audioManager.VolumeDown();
            }
        }
    }
}