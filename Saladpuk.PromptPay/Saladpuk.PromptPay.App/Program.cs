using System;
using System.Text.Json;
using Saladpuk.PromptPay.Facades;

namespace Saladpuk.PromptPay.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // QR Creation
            var billPaymentQR = PPay.StaticQR
                .TaxId("001122334455667")
                .BillerSuffix("02")
                .BillRef1("ref1")
                .BillRef2("ref2")
                .Amount(50)
                .GetBillPaymentQR();
            Console.WriteLine($"Bill Payment: {billPaymentQR}");

            var creditTransferQR = PPay.StaticQR
                .MobileNumber("0914185401")
                .Amount(50)
                .GetCreditTransferQR();
            Console.WriteLine($"Credit Transfer: {creditTransferQR}");

            // QR Reader
            var staticMobileTransferQrCode = "00020101021229370016A000000677010111011300669141854015303764540550.005802TH630401F8";
            Console.WriteLine($"Are equal: {creditTransferQR == staticMobileTransferQrCode}");
            var model = PPay.Reader.ConvertToQrInfo(staticMobileTransferQrCode);
            var content = JsonSerializer.Serialize(model);
            Console.WriteLine(content);
        }
    }
}
