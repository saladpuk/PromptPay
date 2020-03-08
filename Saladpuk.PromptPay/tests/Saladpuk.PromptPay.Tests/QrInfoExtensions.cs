using FluentAssertions;
using Saladpuk.PromptPay.Models;
using emv = Saladpuk.PromptPay.EMVCoValues;

namespace Saladpuk.PromptPay.Tests
{
    public static class QrInfoExtensions
    {
        public static QrInfo InitializeDefault(this QrInfo qr, bool staticQr = true, CurrencyCode currency = CurrencyCode.THB, string country = "TH")
        {
            qr.PayloadFormatIndicator = emv.FirstPayloadIndicatorId;
            qr.PointOfInitiationMethod = staticQr ? emv.Static : emv.Dynamic;
            qr.TransactionCurrency = ((int)currency).ToString();
            qr.CountryCode = country;
            return qr;
        }

        public static void ValidateWith(this QrInfo qr, QrInfo expected, bool skipChecksum = true)
        {
            expected.CRC = skipChecksum ? qr.CRC : expected.CRC;
            qr.Should().BeEquivalentTo(expected);
        }
    }
}
