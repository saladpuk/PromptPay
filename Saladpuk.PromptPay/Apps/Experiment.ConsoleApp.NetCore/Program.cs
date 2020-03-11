using Saladpuk.EMVCo.Contracts;
using Saladpuk.PromptPay.Contracts.Models;
using Saladpuk.PromptPay.Facades;
using Saladpuk.PromptPay.Models;
using System;
using System.Text.Json;

namespace Experiment.ConsoleApp.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // QR Creations
            var billPaymentQR = PPay.StaticQR
                .TaxId("001122334455667")
                .BillerSuffix("02")
                .BillRef1("ref1")
                .BillRef2("ref2")
                .Amount(50)
                .CreateBillPaymentQrCode();
            Console.WriteLine($"Bill Payment: {billPaymentQR}");

            var creditTransferQR = PPay.DynamicQR
                .MobileNumber("0914185401")
                .Amount(50)
                .CreateCreditTransferQrCode();
            Console.WriteLine($"Credit Transfer (mobile): {creditTransferQR}");

            var creditTransferQR2 = PPay.DynamicQR
                .CreateCreditTransferQrCode(new CreditTransfer
                {
                    NationalIdOrTaxId = "1234567890123"
                });
            Console.WriteLine($"Credit Transfer (PID): {creditTransferQR2}");

            // QR Reader
            var staticMobileTransferQrCode = "00020101021229370016A000000677010111011300669141854015303764540550.005802TH630401F8";
            Console.WriteLine($"Are equal: {creditTransferQR == staticMobileTransferQrCode}");
            var model = PPay.Reader.Read(staticMobileTransferQrCode) as PromptPayQrInfo;
            var content = JsonSerializer.Serialize(model);
            Console.WriteLine(content);
        }
    }
}
