using Common;

namespace Hw8.Exercise0.Abstractions;

public interface ICaching
{
    public string CacheCurrencyRates(string cacheFileName, IFileSystemProvider fileSystemProvider);
}
