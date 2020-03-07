namespace Saladpuk.PromptPay
{
    public static class QrBuilderExtensions
    {
        #region Credit Transfer

        public static QrBuilder MobileNumber(this QrBuilder builder, string value)
        {
            builder.creditTransfer.MobileNumber = value;
            return builder;
        }

        public static QrBuilder EWallet(this QrBuilder builder, string value)
        {
            builder.creditTransfer.EWalletId = value;
            return builder;
        }

        public static QrBuilder BankAccount(this QrBuilder builder, string value)
        {
            builder.creditTransfer.BankAccount = value;
            return builder;
        }

        public static QrBuilder OTA(this QrBuilder builder, string value)
        {
            builder.creditTransfer.OTA = value;
            return builder;
        }

        public static QrBuilder MerchantPresentedQR(this QrBuilder builder)
        {
            builder.creditTransfer.MerchantPresentedQR = true;
            return builder;
        }

        public static QrBuilder CustomerPresentedQR(this QrBuilder builder)
        {
            builder.creditTransfer.MerchantPresentedQR = false;
            return builder;
        }

        public static string GetCreditTransferQR(this QrBuilder builder)
            => builder.SetCreditTransfer(builder.creditTransfer).GetQR();

        #endregion Credit Transfer

        #region Bill Payment

        public static QrBuilder BillerSuffix(this QrBuilder builder, string value)
        {
            builder.billPayment.Suffix = value;
            return builder;
        }

        public static QrBuilder BillRef1(this QrBuilder builder, string value)
        {
            builder.billPayment.Reference1 = value;
            return builder;
        }

        public static QrBuilder BillRef2(this QrBuilder builder, string value)
        {
            builder.billPayment.Reference2 = value;
            return builder;
        }

        public static QrBuilder DomesticMerchant(this QrBuilder builder)
        {
            builder.billPayment.DomesticMerchant = true;
            return builder;
        }

        public static QrBuilder CrossBorderMerchant(this QrBuilder builder)
        {
            builder.billPayment.DomesticMerchant = false;
            return builder;
        }

        public static string GetBillPaymentQR(this QrBuilder builder)
            => builder.SetBillPayment(builder.billPayment).GetQR();

        #endregion Bill Payment

        public static QrBuilder NationalId(this QrBuilder builder, string value)
        {
            builder.creditTransfer.NationalIdOrTaxId = value;
            builder.billPayment.NationalIdOrTaxId = value;
            return builder;
        }

        public static QrBuilder TaxId(this QrBuilder builder, string value)
        {
            builder.creditTransfer.NationalIdOrTaxId = value;
            builder.billPayment.NationalIdOrTaxId = value;
            return builder;
        }

        public static QrBuilder Amount(this QrBuilder builder, double amount)
            => builder.SetTransactionAmount(amount);

        private static string GetQR(this QrBuilder builder, ICyclicRedundancyCheck crc = null)
            => builder.SetCyclicRedundancyCheck(crc ?? new SimpleCRC16()).ToString();
    }
}
