/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevisedGestrApp.Droid.Services;
using RevisedGestrApp.Droid.Services.Music;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Unit_Tests.Android
{
    /// <summary>
    /// Tests for SpotifyManager
    /// </summary>
    [TestClass]
    public class ActionHandlerTests
    {
        ActionHandler actionHandler;

        [TestInitialize()]
        public void TestInitialize()
        {
            IMusicManager testMusicManager = new SpotifyManagerMock();
            actionHandler = new ActionHandler(null, null, null, null, new SmsManagerMock());
            actionHandler.RegisterMusicManager(testMusicManager);
        }

        [TestMethod]
        public async Task PerformAction()
        {
            Assert.IsFalse(SpotifyManagerMock.ResumeCalled);
            await actionHandler.PerformAction("Play");
            Assert.IsTrue(SpotifyManagerMock.ResumeCalled);

            Assert.IsFalse(SpotifyManagerMock.PauseCalled);
            await actionHandler.PerformAction("Pause");
            Assert.IsTrue(SpotifyManagerMock.PauseCalled);

            Assert.IsFalse(SpotifyManagerMock.SkipCalled);
            await actionHandler.PerformAction("Skip");
            Assert.IsTrue(SpotifyManagerMock.SkipCalled);
        }
    }
}
