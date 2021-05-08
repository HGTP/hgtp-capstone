/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace RevisedGestrApp.ViewModels
{
    class EmergencyTextViewModel : BaseViewModel
    {
        // Private members
        private readonly IListableDataStore<Contact> contactDataStore;
        private readonly IDataStore<Text> textDataStore;
        private readonly IDataStore<bool> sendGPSStore;

        public ObservableCollection<Contact> Items { get; set; }
        public Text Message { get; set; }
        public String MessageText { get; set; }
        public bool SendGPSCoords { get; set; }
        public EmergencyTextViewModel (
            IListableDataStore<Contact> contactsDataStore = null,
            IDataStore<Text> textsDataStore = null,
            IDataStore<bool> sendGPSStore = null)
        {
            Title = "Emergency Texts";
            this.contactDataStore = contactsDataStore ?? new SOSContactStore();
            this.textDataStore = textsDataStore ?? new SOSTextStore();
            this.sendGPSStore = sendGPSStore ?? new SendGPSBoolStore();
            Task.Run(InitSettings).Wait();
        }


        /// <summary>
        /// Asynchronously retrieves settings from persistent storage and updates 
        /// the view. The loading spinner is active while this is working.
        /// </summary>
        private async void InitSettings()
        {
            Items = new ObservableCollection<Contact>();
            Message = new Text { TextMsg = "" };
            MessageText = "";
            SendGPSCoords = false;
            //get items from memory if they exist
            ObservableCollection<Contact> newItems;
            Text newText;
            try
            {
                newItems = await GetContactsAsync();

                foreach (Contact c in newItems)
                {
                    Items.Add(c);
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                await contactDataStore.ReplaceAsync(Items);
            }

            try
            {
                newText = await textDataStore.GetAsync("SOS_text");
                Message.TextMsg = newText.TextMsg;
                MessageText = newText.TextMsg;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                await textDataStore.AddAsync(Message);
            }

            try
            {
                SendGPSCoords = await sendGPSStore.GetAsync("Send_GPS");
            }
            catch (KeyNotFoundException)
            {
                await sendGPSStore.AddAsync(SendGPSCoords);
            }

            //LoadingSettings = false;
            OnPropertyChanged("Items");
            OnPropertyChanged("Message");
            OnPropertyChanged("MessageText");
            OnPropertyChanged("SendGPSCoords");

            System.Diagnostics.Debug.WriteLine("3");
        }

        private async Task<ObservableCollection<Contact>> GetContactsAsync()
        {
            return await contactDataStore.ListAsync();
        }

        /// <summary>
        /// Provided for testing
        /// </summary>
        /// <returns></returns>
        private string GetText()
        {
            return MessageText;
        }

        /// <summary>
        /// Provided for testing
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Contact> GetContacts()
        {
            return Items;
        }

        public void AddToList(Contact c)
        {
            Items.Add(c);
            OnPropertyChanged("Items");
        }

        internal void RemoveFromList(Contact c)
        {
            Items.Remove(c);
        }

        public void UpdateContacts()
        {
            contactDataStore.ReplaceAsync(Items);
        }

        public void UpdateMessage()
        {
            Message.TextMsg = MessageText;
            textDataStore.UpdateAsync(Message);
        }

        public void UpdateGPSBool(bool sendGPS)
        {
            SendGPSCoords = sendGPS;
            sendGPSStore.UpdateAsync(SendGPSCoords);
        }
    }
}
