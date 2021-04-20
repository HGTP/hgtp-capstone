/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Threading.Tasks;

namespace RevisedGestrApp.Droid.Services.Music
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMusicManager
    {
        Task Init();
        Task<bool> IsPlaying();
        Task<bool> Resume();
        Task<bool> Pause();
        Task<bool> Skip();

    }
}