using Common;
using Hw8.Exercise0.Abstractions;

namespace Hw8.Exercise0.Logic;

internal class OldCache : ICaching
{
    public string CacheCurrencyRates(string cacheFileName, IFileSystemProvider fileSystemProvider)
    {
        if (string.IsNullOrEmpty(cacheFileName))
            throw new ArgumentNullException(nameof(cacheFileName));

        using var cacheStream = fileSystemProvider.Read(cacheFileName);
        using var sr = new StreamReader(cacheStream);

        return sr.ReadToEnd();

    }
}
