using Saladpuk.EMVCo.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Saladpuk.EMVCo.Models
{
    /// <summary>
    /// รายละเอียดของ QR
    /// </summary>
    public class QrInfo : IQrInfo
    {
        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับรายละเอียดของ QR
        /// </summary>
        /// <param name="segments">ส่วนประกอบทั้งหมดของ QR code</param>
        public QrInfo(List<IQrDataObject> segments = null)
        {
            Segments = segments ?? new List<IQrDataObject>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ดึงข้อมูลจากส่วนประกอบ โดยการระบุประเภทข้อมูลที่ต้องการ
        /// </summary>
        /// <param name="identifier">ประเภทข้อมูลที่ต้องการอ่าน</param>
        protected string GetSegmentValue(QrIdentifier identifier)
            => Segments?.LastOrDefault(it => it.IdByConvention == identifier)?.Value ?? null;

        #endregion Methods

        #region IQRInfo members

        /// <summary>
        /// ส่วนประกอบทั้งหมดของ QR code
        /// </summary>
        public IList<IQrDataObject> Segments { get; set; }

        /// <summary>
        /// เวอร์ชั่นของการทำธุรกรรม
        /// </summary>
        public string PayloadFormatIndicator => GetSegmentValue(QrIdentifier.PayloadFormatIndicator);

        /// <summary>
        /// รหัสรูปแบบของการทำธุรกรรม
        /// </summary>
        public string PointOfInitiationMethod => GetSegmentValue(QrIdentifier.PointOfInitiationMethod);

        /// <summary>
        /// รหัสรายละเอียดข้อมูลของ Merchant
        /// </summary>
        public string MerchantAccountInformation => GetSegmentValue(QrIdentifier.MerchantAccountInformation);

        /// <summary>
        /// รหัสหมวดของ Merchant ตามมาตรฐาน ISO 18245
        /// </summary>
        public string MerchantCategoryCode => GetSegmentValue(QrIdentifier.MerchantCategoryCode);

        /// <summary>
        /// รหัสสกุลเงินที่ใช้ในการทำธุรกรรม
        /// </summary>
        public string TransactionCurrency => GetSegmentValue(QrIdentifier.TransactionCurrency);

        /// <summary>
        /// กำหนดจำนวนเงินที่จะเรียกเก็บ
        /// </summary>
        public string TransactionAmount => GetSegmentValue(QrIdentifier.TransactionAmount);

        /// <summary>
        /// ทิปที่ฝั่งจ่ายเงินพอใจจะมอบให้
        /// </summary>
        public string TipOrConvenienceIndicator => GetSegmentValue(QrIdentifier.TipOrConvenienceIndicator);

        /// <summary>
        /// ค่าธรรมเนียมที่ถูกเรียกเก็บแบบตายตัว
        /// </summary>
        public string ValueOfConvenienceFeeFixed => GetSegmentValue(QrIdentifier.ValueOfConvenienceFeeFixed);

        /// <summary>
        /// ค่าธรรมเนียมที่ถูกเรียกเก็บแบบเป็นเปอร์เซ็นต์
        /// </summary>
        public string ValueOfConvenienceFeePercentage => GetSegmentValue(QrIdentifier.ValueOfConvenienceFeePercentage);

        /// <summary>
        /// รหัสประเทศของร้านค้า (ตามมาตรฐาน ISO 3166)
        /// </summary>
        public string CountryCode => GetSegmentValue(QrIdentifier.CountryCode);

        /// <summary>
        /// ชื่อ Merchant
        /// </summary>
        public string MerchantName => GetSegmentValue(QrIdentifier.MerchantName);

        /// <summary>
        /// รหัสเมืองที่ตั้งของ Merchant
        /// </summary>
        public string MerchantCity => GetSegmentValue(QrIdentifier.MerchantCity);

        /// <summary>
        /// รหัสไปรษณีย์ของ Merchant
        /// </summary>
        public string PostalCode => GetSegmentValue(QrIdentifier.PostalCode);

        /// <summary>
        /// รหัสรายละเอียดเพิ่มเติมอื่นๆ
        /// </summary>
        public string AdditionalData => GetSegmentValue(QrIdentifier.AdditionalData);

        /// <summary>
        /// รหัสในการตรวจสอบความถูกต้องของข้อมูล
        /// </summary>
        public string CRC => GetSegmentValue(QrIdentifier.CRC);

        /// <summary>
        /// รายละเอียดเพิ่มเติมต่างๆของ Merchant
        /// </summary>
        public string MerchantInformationLanguageTemplate => GetSegmentValue(QrIdentifier.MerchantInformationLanguageTemplate);

        /// <summary>
        /// รหัสข้อมูลอื่นๆ
        /// </summary>
        public string RFU => GetSegmentValue(QrIdentifier.RFU);

        #endregion IQRInfo members
    }
}
