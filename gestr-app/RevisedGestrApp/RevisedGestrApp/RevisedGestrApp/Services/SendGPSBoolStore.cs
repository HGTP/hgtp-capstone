using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RevisedGestrApp.Services
{
    public class SendGPSBoolStore : IDataStore<bool>
    {
        public async Task AddAsync(bool data)
        {
            Application.Current.Properties["Send_GPS"] = data;
            await Application.Current.SavePropertiesAsync();
        }

        public Task DeleteAsync(string id)
        {
            return Task.Run(() => Application.Current.Properties["Send_GPS"] = null);
        }

        public Task<bool> GetAsync(string id)
        {
            return Task.Run(() =>
                {
                    return (bool)Application.Current.Properties["Send_GPS"];
                });
        }

        public async Task UpdateAsync(bool data)
        {
            Application.Current.Properties["Send_GPS"] = data;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
