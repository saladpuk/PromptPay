namespace Saladpuk.PromptPay.Facades
{
    public class PPay
    {
        public static QrBuilder StaticQR
            => initializeQrBuilder().SetStaticQR();

        public static QrBuilder DynamicQR
            => initializeQrBuilder().SetDynamicQR();

        private static QrBuilder initializeQrBuilder()
            => new QrBuilder()
                .DomesticMerchant()
                .MerchantPresentedQR()
                .SetPayloadFormatIndicator()
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th");
    }
}
