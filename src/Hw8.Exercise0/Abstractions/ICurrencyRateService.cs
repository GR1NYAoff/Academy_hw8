namespace Hw8.Exercise0.Abstractions;

public interface ICurrencyRateService
{
    public string GetCurrencyRates(string url, HttpClient client);
}
