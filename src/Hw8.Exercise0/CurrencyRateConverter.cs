using System.Text.Json;
using Common;
using Hw8.Exercise0.Abstractions;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0;

public class CurrencyRateConverter : IConverter
{
    private readonly string? _today;
    public CurrencyRateConverter(string today)
    {
        _today = today;
    }
    public ReturnCode Convert(string jsonCurrentRate, string currentCurrency, string desiredСurrency, decimal sum)
    {
        var currentRate = JsonSerializer.Deserialize<CurrencyRate[]>(jsonCurrentRate);

        decimal current = default;
        decimal desired = default;

        if (currentCurrency == "UAH")
            current = 1;
        else if (desiredСurrency == "UAH")
            desired = 1;

        for (var i = 0; i < currentRate?.Length; i++)
        {
            if (currentRate[i].Cc == currentCurrency)
            {
                current = currentRate[i].Rate;
            }
            else if (currentRate[i].Cc == desiredСurrency)
            {
                desired = currentRate[i].Rate;
            }
        }

        if (current == default || desired == default)
            return ReturnCode.InvalidArgs;

        ShowResult(current, desired, sum, currentCurrency, desiredСurrency);

        return ReturnCode.Success;
    }
    private void ShowResult(decimal current, decimal desired, decimal sum, string currentCurrency, string desiredСurrency)
    {
        var rate = current / desired;
        var result = sum * rate;

        Console.WriteLine("Rate {0}:{1} = {2}, Date: {3}, Sum: {4}", currentCurrency, desiredСurrency, rate, _today, result);
    }
}
