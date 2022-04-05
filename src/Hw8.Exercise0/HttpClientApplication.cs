using System.Globalization;
using Common;
using Hw8.Exercise0.Abstractions;
using Hw8.Exercise0.Logic;
using RichardSzalay.MockHttp;

namespace Hw8.Exercise0;

public class HttpClientApplication
{
    private static readonly string _today = DateTime.Today.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

    private readonly HttpClient _httpClient;
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly ICurrencyRateService _currencyRateService;

    public HttpClientApplication(MockHttpMessageHandler httpMessageHandler,
                                 IFileSystemProvider fileSystemProvider,
                                 ICurrencyRateService currencyRateService)
    {
        _httpClient = httpMessageHandler.ToHttpClient() ?? throw new ArgumentException(nameof(_httpClient));
        _fileSystemProvider = fileSystemProvider ?? throw new ArgumentException(nameof(_fileSystemProvider));
        _currencyRateService = currencyRateService ?? throw new ArgumentException(nameof(_fileSystemProvider));
    }

    /// <summary>
    /// Runs http client app.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>
    /// Returns <see cref="ReturnCode.Success"/> in case of successful exchange calculation.
    /// Returns <see cref="ReturnCode.InvalidArgs"/> in case of invalid <paramref name="args"/>.
    /// Returns <see cref="ReturnCode.Error"/> in case of error <paramref name="args"/>.
    /// </returns>
    public ReturnCode Run(params string[] args)
    {
        if (args.Length != 3)
            return ReturnCode.InvalidArgs;

        var currentCurrency = args[0].ToUpper(CultureInfo.InvariantCulture);
        var desiredСurrency = args[1].ToUpper(CultureInfo.InvariantCulture);
        var parseSum = decimal.TryParse(args[2], out var sum);

        if (!parseSum || sum < 0)
            return ReturnCode.InvalidArgs;

        var cacheManager = new CacheManager(_fileSystemProvider);

        if (!cacheManager.CacheExists())
            cacheManager.SetCaching(new UpdatedCache(_httpClient, _currencyRateService));

        cacheManager.Cache();

        if (!cacheManager.FreshExchangeRate(_today))
        {
            cacheManager.SetCaching(new UpdatedCache(_httpClient, _currencyRateService));
            cacheManager.Cache();
        }

        var jsonCurrentRate = cacheManager.TemporaryCache;

        var converter = new CurrencyRateConverter(_today);

        return converter.Convert(jsonCurrentRate, currentCurrency, desiredСurrency, sum);

    }

}
