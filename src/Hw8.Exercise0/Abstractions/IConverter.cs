using Common;

namespace Hw8.Exercise0.Abstractions;

public interface IConverter
{
    public ReturnCode Convert(string jsonCurrentRate,
                              string currentCurrency,
                              string desiredСurrency,
                              decimal sum);
}
