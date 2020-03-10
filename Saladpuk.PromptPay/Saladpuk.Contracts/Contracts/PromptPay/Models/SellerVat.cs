namespace Saladpuk.Contracts.PromptPay.Models
{
    /// <summary>
    /// ข้อมูลภาษี
    /// </summary>
    public class SellerVat
    {
        /// <summary>
        /// รหัสอ้างอิงตามระบบ ITMX
        /// </summary>
        public string SellerTaxBranchId { get; set; }
        /// <summary>
        /// ภาษีที่เรียกเก็บในรูปแบบเปอร์เซ็นต์ (ต้องระบุทศนิยม 2 ตำแหน่งเสมอ)
        /// </summary>
        /// <remarks>
        /// ภาษี 10% จะต้องกำหนดเป็น "10.00"
        /// กรณีที่ไม่มีทศนิยมถือว่าไม่ถูกรูปแบบ เช่น "10"
        /// </remarks>
        public string VATRate { get; set; }
        /// <summary>
        /// มูลค่าภาษีที่แสดงอยู่บนใบเรียกเก็บ
        /// </summary>
        public string VATAmount { get; set; }
    }
}
