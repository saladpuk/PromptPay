using ppay = Saladpuk.Contracts.PromptPay.PromptPayCodeConventions;
namespace Saladpuk.Contracts.PromptPay.Models
{
    /// <summary>
    /// ข้อมูลการขอเรียกเก็บเงินสำหรับผู้ประกอบการ
    /// </summary>
    public class BillPayment
    {
        /// <summary>
        /// ถ้าเป็นการใช้ภายในประเทศจะมีค่าเป็น "A000000677010112"
        /// ถ้าเป็นการใช้ข้ามประเทศจะมีค่าเป็น "A000000677012006"
        /// </summary>
        public string AID => CrossBorderMerchantQR ? ppay.CrossBorderMerchant : ppay.DomesticMerchant;

        /// <summary>
        /// เลขทะเบียนนิติบุคคล หรือ เลขประจำตัวผู้เสียภาษีของร้านค้า
        /// </summary>
        public string BillerId { get; set; }

        /// <summary>
        /// เลขผู้ให้บริการที่เป็นตัวแทนในการรับชำระเงิน
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// รหัสอ้างอิง 1
        /// </summary>
        public string Reference1 { get; set; }

        /// <summary>
        /// รหัสอ้างอิง 2
        /// </summary>
        public string Reference2 { get; set; }

        /// <summary>
        /// เป็นการใช้ข้ามประเทศหรือไม่ ?
        /// </summary>
        public bool CrossBorderMerchantQR { get; set; }
    }
}
