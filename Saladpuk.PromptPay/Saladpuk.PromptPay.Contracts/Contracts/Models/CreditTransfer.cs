using ppay = Saladpuk.PromptPay.Contracts.PromptPayCodeConventions;

namespace Saladpuk.PromptPay.Contracts.Models
{
    /// <summary>
    /// ข้อมูลการโอนเงินผ่านบริการพร้อมเพย
    /// </summary>
    public class CreditTransfer
    {
        /// <summary>
        /// ถ้าเป็นรูปแบบร้านเป็นผู้แสดง QR ให้ลูกค้าสแกนจะมีค่าเป็น "A000000677010111"
        /// ถ้าเป็นรูปแบบลูกค้าเป็นผู้แสดง QR Code ให้ร้านค้าสแกนจะมีค่าเป็น "A000000677010114"
        /// </summary>
        public string AID => CustomerPresentedQR ? ppay.CustomerPresented : ppay.MerchantPresented; // TODO: ไม่รู้ว่ามันคืออะไร ถ้าใครรู้ก็บอกด้วย

        /// <summary>
        /// เบอร์มือถือของผู้รับเงิน
        /// </summary>
        /// <remarks>
        /// ต้องเป็นรหัสตามรูปแบบนี้ 0066XXXXXXXXX
        /// </remarks>
        public string MobileNumber { get; set; }

        /// <summary>
        /// รหัสประจำตัวประชาชน หรือ เลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string NationalIdOrTaxId { get; set; }

        /// <summary>
        /// รหัสกระเป๋าเงินอิเล็กทรอนิกส์
        /// </summary>
        public string EWalletId { get; set; }

        /// <summary>
        /// รหัสบัญชีธนาคารผู้รับเงิน
        /// </summary>
        /// <remarks>
        /// Bank code(3 digit) + account no.
        /// </remarks>
        public string BankAccount { get; set; }

        /// <summary>
        /// ถ้าเป็น QR รูปแบบร้านค้าเป็นผู้แสดง QR ให้ลูกค้าสแกน ต้องกำหนดค่านี้ด้วย
        /// </summary>
        public string OTA { get; set; } // TODO: ไม่รู้ว่ามันคืออะไร ถ้าใครรู้ก็บอกด้วย

        /// <summary>
        /// เป็นรูปแบบลูกค้าเป็นผู้แสดง QR Code ให้ร้านค้าสแกนหรือไม่ ?
        /// </summary>
        public bool CustomerPresentedQR { get; set; }
    }
}
