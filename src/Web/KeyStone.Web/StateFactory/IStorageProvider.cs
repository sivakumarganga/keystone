namespace KeyStone.Web.StateFactory
{
    public interface IStorageProvider
    {
        Task<T?> GetItemAsync<T>(string key);
        ValueTask<bool> RemoveItemAsync(string key);
        ValueTask<bool> SaveItemAsync<T>(string key, T value);

    }

    public interface ILocalStorageProvider : IStorageProvider
    {
    }
}
