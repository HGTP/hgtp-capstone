/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System.Threading.Tasks;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// Represents a store where you can interact with some data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataStore<T>
    {
        Task AddAsync(T data);
        Task UpdateAsync(T data);
        Task DeleteAsync(string id);
        Task<T> GetAsync(string id);
    }
}
