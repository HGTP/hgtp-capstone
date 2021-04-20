/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using RevisedGestrApp.Util;

namespace RevisedGestrApp.Droid.Services.Music
{
    /// <summary>
    /// Manages Spotify music controls.
    /// </summary>
    public class SpotifyManager : IMusicManager
    {
        public bool IsAuthenticated = false;
        private static readonly EmbedIOAuthServer server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
        private string verifier, challenge;
        private ISpotifyClient client;
        private PKCETokenResponse token;

        /// <summary>
        /// Empty Spotify constructor.
        /// </summary>
        public SpotifyManager() { }

        /// <summary>
        /// Spotify Manager with Spotify Client
        /// </summary>
        /// <param name="client">a Spotify client for Spotify controls</param>
        public SpotifyManager(ISpotifyClient client)
        {
            this.token = null;
            this.client = client;
        }

        /// <summary>
        /// Completes the Spotify authorization process 
        /// </summary>
        public async Task Init()
        {
            var (v, c) = PKCEUtil.GenerateCodes();
            await server.Start();
            challenge = (v, c).c;
            verifier = (v, c).v;

            server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await server.Stop();
                token = await new OAuthClient().RequestToken(
                  new PKCETokenRequest("<INSERT_CLIENT_ID>", response.Code, server.BaseUri, verifier));
                client = new SpotifyClient(token);
                IsAuthenticated = true;
                bool hasPremium = await HasPremium();
                MessagingCenter.Send(new SpotifyLoginMessage("LoginSuccess", hasPremium), "LoginSuccess");
            };

            var loginRequest = new LoginRequest(
                new Uri("http://localhost:5000/callback"),
                "<INSERT_CLIENT_ID>",
                LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = challenge,
                Scope = new[] 
                {
                    Scopes.UserModifyPlaybackState,
                    Scopes.AppRemoteControl,
                    Scopes.UserReadCurrentlyPlaying,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserLibraryRead,
                    Scopes.UserReadRecentlyPlayed,
                    Scopes.UserReadPrivate,
                },
            };

            var uri = loginRequest.ToUri();
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // TODO: log error to app center.
            }
        }

        /// <summary>
        /// Checks the current Spotify client to see if music is playing. 
        /// </summary>
        /// <returns>true if music stream can be accessed and is playing, false otherwise</returns>
        public async Task<bool> IsPlaying()
        {
            try
            {
                await RefreshAuthentication();
                var playback = await client.Player.GetCurrentPlayback();
                return (playback != null && playback.IsPlaying == true);
            }
            catch (APIUnauthorizedException ex)
            {
                // TODO: either prompt for login or refresh token
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (APIException ex)
            {
                // TODO: determine if this is due to not having premium
                Console.WriteLine(ex.Message);
                return false;
            }     
        }

        /// <summary>
        /// Refreshes Spotify authentication to continue having access to Spotify music controls
        /// </summary>
        /// <returns>Task for refreshing token</returns>
        private async Task RefreshAuthentication()
        {
            if (token == null)
            {
                await Task.Run(Init);
            }
            else if (token != null && token.IsExpired)
            {
                PKCETokenRefreshRequest refreshRequest = new PKCETokenRefreshRequest("<INSERT_CLIENT_ID>", token.RefreshToken);
                PKCETokenResponse refreshResponse = await new OAuthClient().RequestToken(refreshRequest);
                client = new SpotifyClient(refreshResponse.AccessToken);
            }
        }

        /// <summary>
        /// Pauses Spotify music if music can be accessed and still currently being played
        /// </summary>
        /// <returns>true if music could be accessed and was paused, false otherwise</returns>
        public async Task<bool> Pause()
        {
            bool result = true;
            if (await IsPlaying())
            {
                result = await client.Player.PausePlayback();
            }
            return result;
        }

        /// <summary>
        /// Plays Spotify music if music can be accessed, there is a song in the queue, and not currently being played
        /// </summary>
        /// <returns>true if music could be accessed and was resumed, false otherwise</returns>
        public async Task<bool> Resume()
        {
            bool result = true;
            if (!await IsPlaying())
            {
                result = await client.Player.ResumePlayback();
            }
            return result;
        }

        /// <summary>
        /// Skips Spotify song if music can be accessed and still currently being played
        /// </summary>
        /// <returns>true if music could be accessed and was skipped, false otherwise</returns>
        public async Task<bool> Skip()
        {
            bool result = false;
            if (await IsPlaying())
            {
                result = await client.Player.SkipNext();
            }
            return result;
        }

        /// <summary>
        /// Checks if the current user has premium
        /// </summary>
        /// <returns>true if the user has premium, false otherwise</returns>
        private async Task<bool> HasPremium()
        {
            bool result = false;
            if (client != null && IsAuthenticated)
            {
                PrivateUser currentUser = await client.UserProfile.Current();
                result = (currentUser.Product == "premium");
            }
            return result;
        }
    }
}