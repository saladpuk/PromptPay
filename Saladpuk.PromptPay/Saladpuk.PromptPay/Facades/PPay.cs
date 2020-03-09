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

        public static IQrReader Reader
            => new PromptPayQrReader();

        private static IPromptPayBuilder initializeQrBuilder()
            => new PromptPayQrBuilder()
                .DomesticMerchant()
                .MerchantPresentedQR()
                .SetPayloadFormatIndicator()
                .SetCurrencyCode(CurrencyCode.THB)
                .SetCountryCode("th");
    }
}
