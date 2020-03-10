using System.Collections.Generic;

namespace Saladpuk.Contracts
{
    /// <summary>
    /// มาตรฐานรายละเอียดของ QR
    /// </summary>
    public interface IQrInfo
    {
        /// <summary>
        /// ส่วนประกอบทั้งหมดของ QR code
        /// </summary>
        IList<IQrDataObject> Segments { get; set; }
        /// <summary>
        /// เวอร์ชั่นของการทำธุรกรรม
        /// </summary>
        string PayloadFormatIndicator { get; }
        /// <summary>
        /// รหัสรูปแบบของการทำธุรกรรม
        /// </summary>
        string PointOfInitiationMethod { get; }
        /// <summary>
        /// รหัสรายละเอียดข้อมูลของ Merchant
        /// </summary>
        string MerchantAccountInformation { get; }
        /// <summary>
        /// รหัสหมวดของ Merchant ตามมาตรฐาน ISO 18245
        /// </summary>
        string MerchantCategoryCode { get; }
        /// <summary>
        /// รหัสสกุลเงินที่ใช้ในการทำธุรกรรม
        /// </summary>
        string TransactionCurrency { get; }
        /// <summary>
        /// กำหนดจำนวนเงินที่จะเรียกเก็บ
        /// </summary>
        string TransactionAmount { get; }
        /// <summary>
        /// ทิปที่ฝั่งจ่ายเงินพอใจจะมอบให้
        /// </summary>
        string TipOrConvenienceIndicator { get; }
        /// <summary>
        /// ค่าธรรมเนียมที่ถูกเรียกเก็บแบบตายตัว
        /// </summary>
        string ValueOfConvenienceFeeFixed { get; }
        /// <summary>
        /// ค่าธรรมเนียมที่ถูกเรียกเก็บแบบเป็นเปอร์เซ็นต์
        /// </summary>
        string ValueOfConvenienceFeePercentage { get; }
        /// <summary>
        /// รหัสประเทศของร้านค้า (ตามมาตรฐาน ISO 3166)
        /// </summary>
        string CountryCode { get; }
        /// <summary>
        /// ชื่อ Merchant
        /// </summary>
        string MerchantName { get; }
        /// <summary>
        /// รหัสเมืองที่ตั้งของ Merchant
        /// </summary>
        string MerchantCity { get; }
        /// <summary>
        /// รหัสไปรษณีย์ของ Merchant
        /// </summary>
        string PostalCode { get; }
        /// <summary>
        /// รหัสรายละเอียดเพิ่มเติมอื่นๆ
        /// </summary>
        string AdditionalData { get; }
        /// <summary>
        /// รหัสในการตรวจสอบความถูกต้องของข้อมูล
        /// </summary>
        string CRC { get; }
        /// <summary>
        /// รายละเอียดเพิ่มเติมต่างๆของ Merchant
        /// </summary>
        string MerchantInformationLanguageTemplate { get; }
        /// <summary>
        /// รหัสข้อมูลอื่นๆ
        /// </summary>
        string RFU { get; }
        string UnreservedTemplates { get; }
    }
}
