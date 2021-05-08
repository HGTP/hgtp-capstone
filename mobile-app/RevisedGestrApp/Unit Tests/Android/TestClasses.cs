/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevisedGestrApp.Droid.Services.Music;
using RevisedGestrApp.Droid.Services.PhoneCalls;
using RevisedGestrApp.Droid.Services.Texts;
using RevisedGestrApp.Droid.Util;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;

namespace Unit_Tests.Android
{
    public class TelecomManagerMock : ITelecomManager
    {
        public bool AcceptedCall { get; private set; }
        public void AcceptRingingCall()
        {
            AcceptedCall = true;
        }

        public void DeclineRingingCall()
        {
            AcceptedCall = false;
        }
    }

    public class SmsManagerMock : ISmsManager
    {
        public int AmountOfTexts { get; private set; }

        public void ReadRecentText()
        {
            throw new NotImplementedException();
        }

        public void SendTextMessage(string number,  string text)
        {
          
           AmountOfTexts++;
            
        }
    }

    public class ContextMock : IContext
    {
        public bool CalledGetSystemService { get; private set; }
        private TelecomManagerMock telecomManager;
        public ContextMock(TelecomManagerMock telecomManager)
        {
            this.telecomManager = telecomManager;
        }
        public ITelecomManager GetSystemService()
        {
            CalledGetSystemService = true;
            return telecomManager;
        }
    }

    public class VersionControlMock : IVersionControl
    {
        public bool ReturnValue { get; set; }
        public bool BuildVersionIsValid()
        {
            return ReturnValue;
        }
    }

    class SpotifyManagerMock : IMusicManager
    {
        public static bool ResumeCalled { get; set; } = false;
        public static bool PauseCalled { get; set; } = false;
        public static bool SkipCalled { get; set; } = false;

        public Task Init()
            => throw new NotImplementedException();

        public Task<bool> IsPlaying()
            => throw new NotImplementedException();

        async public Task<bool> Pause()
            => PauseCalled = true;

        async public Task<bool> Resume()
            => ResumeCalled = true;

        async public Task<bool> Skip()
            => SkipCalled = true;
    }

    class SpotifyClientMock : ISpotifyClient
    {
        IPaginator ISpotifyClient.DefaultPaginator => throw new NotImplementedException();

        IUserProfileClient ISpotifyClient.UserProfile => throw new NotImplementedException();

        IBrowseClient ISpotifyClient.Browse => throw new NotImplementedException();

        IShowsClient ISpotifyClient.Shows => throw new NotImplementedException();

        IPlaylistsClient ISpotifyClient.Playlists => throw new NotImplementedException();

        ISearchClient ISpotifyClient.Search => throw new NotImplementedException();

        IFollowClient ISpotifyClient.Follow => throw new NotImplementedException();

        ITracksClient ISpotifyClient.Tracks => throw new NotImplementedException();

        IPlayerClient ISpotifyClient.Player
        {
            get
            {
                return new PlayerMock();
            }
        }

        IAlbumsClient ISpotifyClient.Albums => throw new NotImplementedException();

        IArtistsClient ISpotifyClient.Artists => throw new NotImplementedException();

        IPersonalizationClient ISpotifyClient.Personalization => throw new NotImplementedException();

        IEpisodesClient ISpotifyClient.Episodes => throw new NotImplementedException();

        ILibraryClient ISpotifyClient.Library => throw new NotImplementedException();

        IResponse ISpotifyClient.LastResponse => throw new NotImplementedException();

        public Task<Paging<T>> NextPage<T>(Paging<T> paging)
        {
            throw new NotImplementedException();
        }

        public Task<CursorPaging<T>> NextPage<T>(CursorPaging<T> cursorPaging)
        {
            throw new NotImplementedException();
        }

        public Task<TNext> NextPage<T, TNext>(IPaginatable<T, TNext> paginatable)
        {
            throw new NotImplementedException();
        }

        public Task<Paging<T>> PreviousPage<T>(Paging<T> paging)
        {
            throw new NotImplementedException();
        }

        public Task<TNext> PreviousPage<T, TNext>(Paging<T, TNext> paging)
        {
            throw new NotImplementedException();
        }

        Task<IList<T>> ISpotifyClient.PaginateAll<T>(IPaginatable<T> firstPage, IPaginator paginator)
        {
            throw new NotImplementedException();
        }

        Task<IList<T>> ISpotifyClient.PaginateAll<T, TNext>(IPaginatable<T, TNext> firstPage, Func<TNext, IPaginatable<T, TNext>> mapper, IPaginator paginator)
        {
            throw new NotImplementedException();
        }
    }

    class PlayerMock : IPlayerClient
    {
        public static bool GetCurrentPlaybackResponse { get; set; }
        public static bool GetCurrentPlaybackCalled { get; set; } = false;
        public static bool PausePlaybackCalled { get; set; } = false;
        public static bool ResumePlaybackCalled { get; set; } = false;
        public static bool SkipNextCalled { get; set; } = false;


        public PlayerMock() { }

        Task<bool> IPlayerClient.AddToQueue(PlayerAddToQueueRequest request)
            => throw new NotImplementedException();

        Task<DeviceResponse> IPlayerClient.GetAvailableDevices()
            => throw new NotImplementedException();

        Task<CurrentlyPlaying> IPlayerClient.GetCurrentlyPlaying(PlayerCurrentlyPlayingRequest request)
            => throw new NotImplementedException();

        async Task<CurrentlyPlayingContext> IPlayerClient.GetCurrentPlayback()
        {
            GetCurrentPlaybackCalled = true;
            return new CurrentlyPlayingContext
            {
                IsPlaying = GetCurrentPlaybackResponse,
            };
        }

        Task<CurrentlyPlayingContext> IPlayerClient.GetCurrentPlayback(PlayerCurrentPlaybackRequest request)
            => throw new NotImplementedException();

        Task<CursorPaging<PlayHistoryItem>> IPlayerClient.GetRecentlyPlayed()
            => throw new NotImplementedException();

        Task<CursorPaging<PlayHistoryItem>> IPlayerClient.GetRecentlyPlayed(PlayerRecentlyPlayedRequest request)
            => throw new NotImplementedException();

        async Task<bool> IPlayerClient.PausePlayback()
        {
            PausePlaybackCalled = true;
            return true;
        }

        Task<bool> IPlayerClient.PausePlayback(PlayerPausePlaybackRequest request)
            => throw new NotImplementedException();

        async Task<bool> IPlayerClient.ResumePlayback()
        {
            ResumePlaybackCalled = true;
            return true;
        }

        Task<bool> IPlayerClient.ResumePlayback(PlayerResumePlaybackRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SeekTo(PlayerSeekToRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SetRepeat(PlayerSetRepeatRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SetShuffle(PlayerShuffleRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SetVolume(PlayerVolumeRequest request)
            => throw new NotImplementedException();

        async Task<bool> IPlayerClient.SkipNext()
        {
            SkipNextCalled = true;
            return true;
        }

        Task<bool> IPlayerClient.SkipNext(PlayerSkipNextRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SkipPrevious()
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.SkipPrevious(PlayerSkipPreviousRequest request)
            => throw new NotImplementedException();

        Task<bool> IPlayerClient.TransferPlayback(PlayerTransferPlaybackRequest request)
            => throw new NotImplementedException();
    }
}
