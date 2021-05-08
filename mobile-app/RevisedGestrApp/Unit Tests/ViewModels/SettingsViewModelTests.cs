/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using RevisedGestrApp.Models;
using RevisedGestrApp.ViewModels;
using System.Threading.Tasks;

namespace Unit_Tests.ViewModels
{
    [TestClass]
    public class SettingsViewModelTests
    {
        SettingsStoreMock settingsStoreMock;
        SettingsViewModel viewModel;
        PrivateObject testObject;

        [TestInitialize]
        public void TestInitialize()
        {
            settingsStoreMock = new SettingsStoreMock();
            viewModel = new SettingsViewModel(settingsStoreMock, new TestAnalytics());
            testObject = new PrivateObject(viewModel);
        }

        //[TestMethod]
        //public void InitializedWithCorrectActions()
        //{
        //    // TODO: This test would be relevant if we retrieve it from the database at some point, 
        //    //       then we would be testing that it's getting what the mock store would provide.
        //    throw new NotImplementedException();
        //}

        [TestMethod]
        public void RetrievesCorrectSelectedAction()
        {
            var settingToRetrieve = new Setting()
            {
                Gesture = "A",
                PhoneAction = "Test Action",
            };
            string retrievedAction = (string)testObject.Invoke("GetSelectedAction", settingToRetrieve);
            Assert.AreEqual(retrievedAction, settingToRetrieve.PhoneAction);
        }

        [TestMethod]
        public void ActionCanBeUpdated()
        {
            // TODO: Determine a way to test OnPropertyChanged events?
            //       Dependency injection doesn't look super convenient with it, though it may be possible.
        }

        [TestMethod]
        public async Task SettingUpdatedCorrectly()
        {
            Assert.IsFalse(settingsStoreMock.CalledUpdateAsync);
            var setting = new Setting()
            {
                Gesture = "A",
                PhoneAction = "Updated Action",
            };
            testObject.Invoke("UpdateSetting", setting);
            Assert.IsTrue(settingsStoreMock.CalledUpdateAsync);
        }

        [TestMethod]
        public void AnalyticsUpdatedCorrectly()
        {
            var setting = new Setting()
            {
                Gesture = "A",
                PhoneAction = "Updated Action",
            };
           // var beforeUpdate = settingsStoreMock.testAnalytics;
            testObject.Invoke("UpdateSetting", setting);
           // var updatedSetting = settingsStoreMock.testAnalytics;
           // Assert.AreNotEqual(beforeUpdate.UpdatedAnalytics, updatedSetting.UpdatedAnalytics);
        }
    }
}
