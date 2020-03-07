using Saladpuk.PromptPay.Models;
using System;

namespace Saladpuk.PromptPay.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new QrBuilder()
                //.SetBillPayment(new BillPayment("1234567890123", "02", "ref1", "ref2"))
                .SetCreditTransfer(new CreditTransfer(true) { MobileNumber = "01234567891" })
                .SetStaticQR()
                .SetPayloadFormatIndicator()
                .SetTransactionAmount(50.00)
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th")
                .SetCyclicRedundancyCheck(new SimpleCRC16());

            var qrCode = builder.ToString();
            Console.WriteLine($"QR code: {qrCode}");
        }
    }
}
