using System;

namespace Saladpuk.PromptPay.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new QrBuilder()
                .SetStaticQR()
                .SetTransactionAmount(50.00)
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th")
                .SetCyclicRedundancyCheck();

            var code = builder.ToString();
            Console.WriteLine($"Raw: {code}");

            var qrCode = builder.GetQrCode(new SimpleCRC16());
            Console.WriteLine($"QR code: {qrCode}");
        }
    }
}
