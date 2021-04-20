/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// Represents a data store of something that can be represented in a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListableDataStore<T> : IDataStore<T>
    {
        Task<ObservableCollection<T>> ListAsync();
        Task ReplaceAsync(ObservableCollection<T> data);
    }
}
