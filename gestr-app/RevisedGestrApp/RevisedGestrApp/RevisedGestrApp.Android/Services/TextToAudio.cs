/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Essentials;

namespace RevisedGestrApp.Droid.Services
{
    class TextToAudio
    {
        private string message;
        public TextToAudio(string msg)
        {
            message = msg;
            Task.Run(() => SpeakNowDefaultSettings());
        }

        CancellationTokenSource cts;
        public async Task SpeakNowDefaultSettings()
        {
            cts = new CancellationTokenSource();
            await TextToSpeech.SpeakAsync(message, cancelToken: cts.Token);

            // This method will block until utterance finishes.
            CancelSpeech();
        }

        // Cancel speech if a cancellation token exists & hasn't been already requested.
        public void CancelSpeech()
        {
            if (cts?.IsCancellationRequested ?? true)
                return;

            cts.Cancel();
        }
    }
}
