using System;
using Saladpuk.PromptPay.Facades;

namespace Saladpuk.PromptPay.App
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
