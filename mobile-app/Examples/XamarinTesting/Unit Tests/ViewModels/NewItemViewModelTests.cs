using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingSample.ViewModels;

namespace Unit_Tests.ViewModels
{
    [TestClass]
    public class NewItemViewModelTests
    {
        NewItemViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            // Would be good to implement some dependency injection with these view models.
            viewModel = new NewItemViewModel();
        }

        [TestMethod]
        public void SomeTest()
        {
            // Test will work with proper stubbing and dependency injection usage.
        }
    }
}
