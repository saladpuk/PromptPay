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
}
