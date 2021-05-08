using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingSample.Models;
using TestingSample.Services;

namespace Unit_Tests
{

    [TestClass]
    public class MockDataStoreTests
    {
        MockDataStore mockDataStore;

        [TestInitialize]
        public void TestInitialize()
        {
            mockDataStore = new MockDataStore();
        }

        [TestMethod]
        public async Task ItemCanBeAdded()
        {
            var itemToAdd = new Item { Id = "TestId", Text = "Test Text", Description = "Test Description" };
            var itemInDataStore = await mockDataStore.GetItemAsync(itemToAdd.Id);
            Assert.IsNull(itemInDataStore);

            await mockDataStore.AddItemAsync(itemToAdd);
            itemInDataStore = await mockDataStore.GetItemAsync(itemToAdd.Id);
            Assert.IsNotNull(itemInDataStore);
            Assert.AreEqual(itemToAdd.Id, itemInDataStore.Id);
        }
    }
}
