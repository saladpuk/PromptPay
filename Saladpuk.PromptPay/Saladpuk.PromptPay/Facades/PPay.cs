using Saladpuk.Contracts;
using Saladpuk.Contracts.PromptPay;

namespace Saladpuk.PromptPay.Facades
{
    public class PPay
    {
        public static IPromptPayBuilder StaticQR
            => initializeQrBuilder().SetStaticQR();

        public static IPromptPayBuilder DynamicQR
            => initializeQrBuilder().SetDynamicQR();

        public static QrReader Reader
            => new QrReader();

        private static IPromptPayBuilder initializeQrBuilder()
            => new QrBuilder()
                .DomesticMerchant()
                .MerchantPresentedQR()
                .SetPayloadFormatIndicator()
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th");
    }
}
