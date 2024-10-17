using Blazored.LocalStorage;

namespace KeyStone.Web.StateFactory
{
    public class LocalStorageService : ILocalStorageProvider
    {
        private readonly ILocalStorageService _localStorage;

        public LocalStorageService(ILocalStorageService localStorageService)
        {
            _localStorage = localStorageService;
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            try
            {
                return await _localStorage.GetItemAsync<T>(key);
            }
            catch
            {
                return default;
            }
        }

        public async ValueTask<bool> RemoveItemAsync(string key)
        {
            try
            {
                await _localStorage.RemoveItemAsync(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async ValueTask<bool> SaveItemAsync<T>(string key, T value)
        {
            try
            {
                await _localStorage.SetItemAsync(key, data: value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
