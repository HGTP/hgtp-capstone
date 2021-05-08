/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class SpotifyManagerTests
    {
        SpotifyManager spotifyManager;

        [TestInitialize()]
        public void TestInitialize()
        {
            ISpotifyClient testClient = new SpotifyClientMock();
            spotifyManager = new SpotifyManager(testClient);
        }

        [TestMethod]
        public async Task IsPlaying()
        {
            Assert.IsFalse(PlayerMock.GetCurrentPlaybackCalled);
            PlayerMock.GetCurrentPlaybackResponse = false;
            var isPlaying = await spotifyManager.IsPlaying();
            Assert.IsFalse(isPlaying);
            Assert.IsTrue(PlayerMock.GetCurrentPlaybackCalled);

            PlayerMock.GetCurrentPlaybackCalled = false;
            PlayerMock.GetCurrentPlaybackResponse = true;
            isPlaying = await spotifyManager.IsPlaying();
            Assert.IsTrue(isPlaying);
            Assert.IsTrue(PlayerMock.GetCurrentPlaybackCalled);
        }

        [TestMethod]
        public async Task Pause()
        {
            Assert.IsFalse(PlayerMock.PausePlaybackCalled);
            PlayerMock.GetCurrentPlaybackResponse = true;
            await spotifyManager.Pause();
            Assert.IsTrue(PlayerMock.PausePlaybackCalled);

            PlayerMock.PausePlaybackCalled = false;
            PlayerMock.GetCurrentPlaybackResponse = false;
            var result = await spotifyManager.Pause();
            Assert.IsFalse(PlayerMock.PausePlaybackCalled);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Resume()
        {
            Assert.IsFalse(PlayerMock.ResumePlaybackCalled);
            PlayerMock.GetCurrentPlaybackResponse = false;
            await spotifyManager.Resume();
            Assert.IsTrue(PlayerMock.ResumePlaybackCalled);

            PlayerMock.ResumePlaybackCalled = false;
            PlayerMock.GetCurrentPlaybackResponse = true;
            var result = await spotifyManager.Resume();
            Assert.IsFalse(PlayerMock.ResumePlaybackCalled);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Skip()
        {
            Assert.IsFalse(PlayerMock.SkipNextCalled);
            PlayerMock.GetCurrentPlaybackResponse = true;
            await spotifyManager.Skip();
            Assert.IsTrue(PlayerMock.SkipNextCalled);

            PlayerMock.SkipNextCalled = false;
            PlayerMock.GetCurrentPlaybackResponse = false;
            var result = await spotifyManager.Skip();
            Assert.IsFalse(PlayerMock.SkipNextCalled);
            Assert.IsFalse(result);
        }
    }
}
