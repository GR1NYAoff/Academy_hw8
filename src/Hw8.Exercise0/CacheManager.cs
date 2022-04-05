using System.Text.Json;
using Common;
using Hw8.Exercise0.Abstractions;
using Hw8.Exercise0.Logic;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0;

public class CacheManager
{
    private static readonly string _cacheFileName = "cache.json";

    private readonly IFileSystemProvider _fileSystemProvider;
    public string TemporaryCache { get; private set; }
    public ICaching? Caching { private get; set; }
    public CacheManager(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
        TemporaryCache = string.Empty;
        Caching = new OldCache();
    }
    public void SetCaching(ICaching caching)
    {
        Caching = caching;
    }
    public void Cache()
    {
        TemporaryCache = Caching!.CacheCurrencyRates(_cacheFileName, _fileSystemProvider);
    }
    public bool CacheExists()
    {
        return _fileSystemProvider.Exists(_cacheFileName);
    }
    public bool FreshExchangeRate(string today)
    {
        if (string.IsNullOrEmpty(TemporaryCache))
            return false;

        var currentRate = JsonSerializer.Deserialize<CurrencyRate[]>(TemporaryCache);

        return !currentRate!.Any(r => r.Exchangedate != today);
    }

}
