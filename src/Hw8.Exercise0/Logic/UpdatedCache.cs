using System.Text;
using Common;
using Hw8.Exercise0.Abstractions;

namespace Hw8.Exercise0.Logic;

public class UpdatedCache : ICaching
{
    private static readonly string _urlApi = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private readonly HttpClient _httpClient;
    private readonly ICurrencyRateService _currencyRateService;
    public UpdatedCache(HttpClient httpClient,
                        ICurrencyRateService currencyRateService)
    {
        _httpClient = httpClient;
        _currencyRateService = currencyRateService;
    }
    public string CacheCurrencyRates(string cacheFileName, IFileSystemProvider fileSystemProvider)
    {
        if (string.IsNullOrEmpty(cacheFileName))
            throw new ArgumentNullException(nameof(cacheFileName));

        var currencyRates = _currencyRateService.GetCurrencyRates(_urlApi, _httpClient);
        _ = WriteCacheAsync(currencyRates, cacheFileName, fileSystemProvider);

        return currencyRates;
    }
    private async Task WriteCacheAsync(string content, string cacheFileName, IFileSystemProvider fileSystemProvider)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var ms = new MemoryStream(bytes);

        await fileSystemProvider.WriteAsync(cacheFileName, ms);
    }
}
