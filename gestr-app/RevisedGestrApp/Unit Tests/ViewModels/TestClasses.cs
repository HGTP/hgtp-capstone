/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RevisedGestrApp.Models;
using RevisedGestrApp.Services;

namespace Unit_Tests.ViewModels
{
    class SettingsStoreMock : IDataStore<Setting>
    {
        public bool CalledUpdateAsync { get; private set; }

        private readonly Setting testSetting = new Setting()
        {
            Gesture = "A",
            PhoneAction = "Test Action",
        };

       // public TestAnalytics testAnalytics { get; private set; }

        public Task AddAsync(Setting data) { return Task.Run(() => { }); }

        public Task DeleteAsync(string id) { return Task.Run(() => { }); }

        public Task<Setting> GetAsync(string id)
        {
            return Task.Run(() =>
            {
                return new Setting()
                {
                    Gesture = testSetting.Gesture,
                    PhoneAction = testSetting.PhoneAction
                };
            });
        }

        public Task UpdateAsync(Setting data)
        {
            CalledUpdateAsync = true;
            return Task.Run(() =>
            {
                testSetting.PhoneAction = data.PhoneAction;
               // testAnalytics.TrackSettingUpdate(data);
            });
        }
    }
    class TextStoreMock : IDataStore<Text>
    {
        public bool CalledUpdateAsync { get; private set; }

        private readonly Text message = new Text() { TextMsg = "Hi" };

        public Task AddAsync(Text data) { return Task.Run(() => { }); }

        public Task DeleteAsync(string id) { return Task.Run(() => { }); }

        public Task<Text> GetAsync(string id)
        {
            return Task.Run(() =>
            {
                return new Text()
                {
                    TextMsg = message.TextMsg
                };
            });
        }

        public Task UpdateAsync(Text data)
        {
            CalledUpdateAsync = true;
            return Task.Run(() =>
            {
                message.TextMsg = data.TextMsg;
            });
        }
    }

    class ContactStoreMock : IListableDataStore<Contact>
    {
        public bool CalledReplaceAsync { get; private set; }

        private readonly ObservableCollection<Contact> items = new ObservableCollection<Contact>() { new Contact() { Name = "Emma", PhoneNumber = "123" } };

        public Task AddAsync(ObservableCollection<Contact> data) { return Task.Run(() => { }); }

        public Task AddAsync(Contact data)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id) { return Task.Run(() => { }); }

        public Task<ObservableCollection<Contact>> GetAsync(string id)
        {
            return Task.Run(() =>
            {

                return new ObservableCollection<Contact>(items);
            });
        }

        public Task<ObservableCollection<Contact>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReplaceAsync(ObservableCollection<Contact> data)
        {
            CalledReplaceAsync = true;
            return null;
        }

        public Task UpdateAsync(ObservableCollection<Contact> data)
        {
            return Task.Run(() =>
            {
                items.Clear();
                foreach (Contact c in data)
                {
                    items.Add(c);
                }
            });
        }

        public Task UpdateAsync(Contact data)
        {
            throw new NotImplementedException();
        }

        Task<Contact> IDataStore<Contact>.GetAsync(string id)
        {
            throw new NotImplementedException();
        }
    }

    class TestAnalytics : IAnalytics
    {
        public bool UpdatedAnalytics { get; private set; }
        public void TrackSettingUpdate(Setting setting)
        {
            Console.WriteLine("In TrackSettingUpdate");
        }
    }
}


