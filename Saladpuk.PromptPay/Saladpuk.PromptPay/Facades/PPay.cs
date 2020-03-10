using Saladpuk.Contracts;
using Saladpuk.Contracts.PromptPay;

namespace Saladpuk.PromptPay.Facades
{
    /// <summary>
    /// พร้อมเพย์
    /// </summary>
    public class PPay
    {
        /// <summary>
        /// สร้าง QR ที่ใช้ซ้ำได้หลายครั้ง
        /// </summary>
        public static IPromptPayBuilder StaticQR
            => initializeQrBuilder().SetStaticQR();

        /// <summary>
        /// สร้าง QR ที่ใช้จ่ายเงินได้เพียงครั้งเดียวแล้วทิ้ง
        /// </summary>
        public static IPromptPayBuilder DynamicQR
            => initializeQrBuilder().SetDynamicQR();

        /// <summary>
        /// ตัวอ่าน QR
        /// </summary>
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
