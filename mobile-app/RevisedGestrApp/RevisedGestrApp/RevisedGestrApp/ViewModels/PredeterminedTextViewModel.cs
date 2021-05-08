/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using RevisedGestrApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RevisedGestrApp.ViewModels
{
    /// <summary>
    /// Based on tutorial by Saleh Qadeer on ListViews https://www.c-sharpcorner.com/article/listview-and-creating-list-in-xamarin/
    /// </summary>
    public class PredeterminedTextViewModel : BaseViewModel
    {
        // Private members
        private readonly IListableDataStore<Contact> contactDataStore;
        private readonly IDataStore<Text> textDataStore;

        public ObservableCollection<Contact> Items { get; set; }
        public Text Message { get; set; }
        public string MessageText { get; set; }

        public PredeterminedTextViewModel(
            IListableDataStore<Contact> contactsDataStore = null,
            IDataStore<Text> textsDataStore = null)
        {
            Title = "Texts";
            this.contactDataStore = contactsDataStore ?? new ContactStore();
            this.textDataStore = textsDataStore ?? new TextStore();
            Task.Run(InitSettings);
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
                newText = await textDataStore.GetAsync("text");
                Message.TextMsg = newText.TextMsg;
                MessageText = newText.TextMsg;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                await textDataStore.AddAsync(Message);
            }
            //LoadingSettings = false;
            OnPropertyChanged("Items");
            OnPropertyChanged("Message");
            OnPropertyChanged("MessageText");
        }

        private async Task<ObservableCollection<Contact>> GetContactsAsync()
        {
            return await contactDataStore.ListAsync();
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
    }
}
