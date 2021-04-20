/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevisedGestrApp.ViewModels;
using RevisedGestrApp.Models;

namespace Unit_Tests.ViewModels
{
    [TestClass]
    public class PredeterminedTextsViewModelTests
    {
        ContactStoreMock contactStoreMock;
        TextStoreMock textStoreMock;
        RevisedGestrApp.ViewModels.PredeterminedTextViewModel viewModel;
        PrivateObject testObject;

        [TestInitialize]
        public void TestInitialize()
        {
            contactStoreMock = new ContactStoreMock();
            textStoreMock = new TextStoreMock();
            viewModel = new PredeterminedTextViewModel(contactStoreMock, textStoreMock);
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
        public void ActionCanBeUpdated()
        {
            // TODO: Determine a way to test OnPropertyChanged events?
            //       Dependency injection doesn't look super convenient with it, though it may be possible.
        }

        [TestMethod]
        public async Task ContactsUpdated()
        {
            Assert.IsFalse(contactStoreMock.CalledReplaceAsync);
            Contact newContact = new Contact() { Name = "Misha", PhoneNumber = "123" };
            testObject.Invoke("AddToList", newContact);
            testObject.Invoke("UpdateContacts");
            Assert.IsTrue(contactStoreMock.CalledReplaceAsync);
        }


        [TestMethod]
        public async Task TextUpdatedCorrectly()
        {
            Assert.IsFalse(textStoreMock.CalledUpdateAsync);
            testObject.Invoke("UpdateMessage");
            Assert.IsTrue(textStoreMock.CalledUpdateAsync);
        }
    }
}