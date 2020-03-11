namespace Saladpuk.PromptPay.Contracts
{
    /// <summary>
    /// รหัสและแท๊กมาตรฐานของ PromptPay
    /// </summary>
    public class PromptPayCodeConventions
    {
        /// <summary>
        /// แท๊ก AID
        /// </summary>
        public const string AIDTag = "00"; // TODO: ไม่รู้ว่ามันคืออะไร ถ้าใครรู้ก็บอกด้วย

        #region Credit Transfer

        /// <summary>
        /// แท๊กการโอนเงินผ่านบริการพร้อมเพย
        /// </summary>
        public const string CreditTransferTag = "29";

        /// <summary>
        /// แท๊กเบอร์มือถือของผู้รับเงิน
        /// </summary>
        public const string MobileTag = "01";

        /// <summary>
        /// แท๊กรหัสประจำตัวประชาชน หรือ เลขประจำตัวผู้เสียภาษี
        /// </summary>
        public const string NationalIdOrTaxIdTag = "02";

        /// <summary>
        /// แท๊กรหัสกระเป๋าเงินอิเล็กทรอนิกส์
        /// </summary>
        public const string EWalletTag = "03";

        /// <summary>
        /// แท๊กรหัสบัญชีธนาคารผู้รับเงิน
        /// </summary>
        public const string BankAccountTag = "04";

        /// <summary>
        /// แท๊ก OTA
        /// </summary>
        public const string OTATag = "05"; // TODO: ไม่รู้ว่ามันคืออะไร ถ้าใครรู้ก็บอกด้วย

        /// <summary>
        /// รูปแบบร้านเป็นผู้แสดง QR ให้ลูกค้าสแกน
        /// </summary>
        public const string MerchantPresented = "A000000677010111";

        /// <summary>
        /// รูปแบบลูกค้าเป็นผู้แสดง QR Code ให้ร้านค้าสแกน
        /// </summary>
        public const string CustomerPresented = "A000000677010114";

        #endregion Credit Transfer

        #region Bill Payment

        /// <summary>
        /// แท๊กการเรียกเก็บเงินสำหรับผู้ประกอบการ
        /// </summary>
        public const string BillPaymentTag = "30";

        /// <summary>
        /// แท๊กเลขทะเบียนนิติบุคคล หรือ เลขประจำตัวผู้เสียภาษีของร้านค้า
        /// </summary>
        public const string BillderIdTag = "01";

        /// <summary>
        /// แท๊กรหัสอ้างอิง 1
        /// </summary>
        public const string Reference1Tag = "02";

        /// <summary>
        /// แท๊กรหัสอ้างอิง 2
        /// </summary>
        public const string Reference2Tag = "03";

        /// <summary>
        /// เป็นการใช้ภายในประเทศ
        /// </summary>
        public const string DomesticMerchant = "A000000677010112";

        /// <summary>
        /// การใช้ข้ามประเทศ
        /// </summary>
        public const string CrossBorderMerchant = "A000000677012006";

        #endregion Bill Payment
    }
}
