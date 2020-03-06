using System;

namespace Saladpuk.PromptPay.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new QrBuilder()
                .Add(QrIdentifier.PointOfInitiationMethod, "12") // No(11), Yes(12)
                .Add(QrIdentifier.TransactionAmount, "50.00")
                .Add(QrIdentifier.TransactionCurrency, "764") // Baht
                .Add(QrIdentifier.CountryCode, "TH")
                .Add(QrIdentifier.CRC, "04")
                ;

            var code = builder.ToString();
            Console.WriteLine($"Raw: {code}");

            var qrCode = builder.GetQrCode(new SimpleCRC16());
            Console.WriteLine($"QR code: {qrCode}");
        }
    }
}
