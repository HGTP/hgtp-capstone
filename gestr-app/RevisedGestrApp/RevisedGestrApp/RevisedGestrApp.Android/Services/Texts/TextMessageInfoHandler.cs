/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;

namespace RevisedGestrApp.Droid.Services.Texts
{
    class TextMessageInfoHandler
    {
        public static async Task<ObservableCollection<Contact>> GetContacts(string gesture)
        {
            IListableDataStore<Contact> regContactStore = new ContactStore();
            IListableDataStore<Contact> sosContactStore = new SOSContactStore();
            var contactsStore = gesture == "predeterminedTexts" ? regContactStore : sosContactStore;
            ObservableCollection<Contact> contacts = await contactsStore.ListAsync();
            return contacts;
        }

        public static async Task<Text> GetText(string keyword)
        {
            IDataStore<Text> regTextStore = new TextStore();
            IDataStore<Text> sosTextStore = new SOSTextStore();
            var textStore = keyword == "text" ? regTextStore : sosTextStore;
            Text text = await textStore.GetAsync(keyword);
            return text;
        }

        public static async Task<bool> GetSendGPS(string keyword)
        {
            var sendGPSStore = new SendGPSBoolStore();
            bool sendGPS = await sendGPSStore.GetAsync(keyword);
            return sendGPS;
        }
    }
}
