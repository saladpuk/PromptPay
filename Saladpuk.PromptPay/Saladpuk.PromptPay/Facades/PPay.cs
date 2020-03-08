namespace Saladpuk.PromptPay.Facades
{
    public class PPay
    {
        public static QrBuilder StaticQR
            => initializeQrBuilder().SetStaticQR();

        public static QrBuilder DynamicQR
            => initializeQrBuilder().SetDynamicQR();

        public static QrReader Reader
            => new QrReader();

        private static QrBuilder initializeQrBuilder()
            => new QrBuilder()
                .DomesticMerchant()
                .MerchantPresentedQR()
                .SetPayloadFormatIndicator()
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th");
    }
}
