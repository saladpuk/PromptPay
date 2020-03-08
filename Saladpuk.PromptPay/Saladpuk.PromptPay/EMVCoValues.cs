namespace Saladpuk.PromptPay
{
    public class EMVCoValues
    {
        public const string FirstPayloadIndicatorId = "01";

        /// <summary>
        /// One time use QR.
        /// </summary>
        public const string Dynamic = "11";

        /// <summary>
        /// Reusable QR.
        /// </summary>
        public const string Static = "12";

        public const int MinContentLength = 5;
        public const int CRCDigits = 4;
    }
}
