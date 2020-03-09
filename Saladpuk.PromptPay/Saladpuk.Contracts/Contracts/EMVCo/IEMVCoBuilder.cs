namespace Saladpuk.Contracts.EMVCo
{
    public interface IEMVCoBuilder
    {
        IEMVCoBuilder Add(string rawData);
        IEMVCoBuilder Add(QrIdentifier identifier, string data);
        IEMVCoBuilder SetStaticQR();
        IEMVCoBuilder SetDynamicQR();
        IEMVCoBuilder SetTransactionAmount(double amount);
        IEMVCoBuilder SetCountryCode(string code);
        IEMVCoBuilder SetCurrencyCode(CurrencyCode code);
        IEMVCoBuilder SetPayloadFormatIndicator(string version = "01");
        IEMVCoBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc);
    }

    public interface IEMVCoBuilder<T> where T : class
    {
        T Add(string rawData);
        T Add(QrIdentifier identifier, string data);
        T SetStaticQR();
        T SetDynamicQR();
        T SetTransactionAmount(double amount);
        T SetCountryCode(string code);
        T SetCurrencyCode(CurrencyCode code);
        T SetPayloadFormatIndicator(string version = "01");
        T SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc);
    }
}
