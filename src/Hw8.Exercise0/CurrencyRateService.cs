using Hw8.Exercise0.Abstractions;

namespace Hw8.Exercise0;

public class CurrencyRateService : ICurrencyRateService
{
    public string GetCurrencyRates(string url, HttpClient client)
    {
        var response = client.GetAsync(url).Result;
        var jsonCurrencyRate = response.Content.ReadAsStringAsync().Result;

        if (string.IsNullOrEmpty(jsonCurrencyRate))
            jsonCurrencyRate = GetCurrencyRatesFromNbu(url);

        return jsonCurrencyRate;

    }
    private string GetCurrencyRatesFromNbu(string url)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var client = new HttpClient();

        var contentStream = client.SendAsync(requestMessage).Result.Content.ReadAsStream();

        using var reader = new StreamReader(contentStream);

        return reader.ReadToEnd();

    }
}
