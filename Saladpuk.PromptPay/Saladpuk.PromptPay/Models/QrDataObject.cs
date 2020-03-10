using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using System;
using System.Linq;
using emv = Saladpuk.Contracts.EMVCo.EMVCoCodeConventions;

namespace Saladpuk.PromptPay.Models
{
    /// <summary>
    /// ตัวเก็บข้อมูล QR Data Object
    /// </summary>
    public class QrDataObject : IQrDataObject
    {
        #region Properties

        /// <summary>
        /// ข้อมูลดิบ
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        /// รหัสประเภทข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// ความยาวของข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        public string Length { get; }

        /// <summary>
        /// ข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// รหัสประเภทข้อมูลตามมาตรฐาน
        /// </summary>
        public QrIdentifier IdByConvention
        {
            get
            {
                if (!Enum.TryParse(Id, out QrIdentifier identifier))
                {
                    throw new ArgumentOutOfRangeException("QR identifier code isn't valid.");
                }

                var id = (int)identifier;
                var isMerchant = emv.MerchantIdRange.Contains(id);
                if (isMerchant)
                {
                    return QrIdentifier.MerchantAccountInformation;
                }

                var isReserved = emv.RFUIdRange.Contains(id);
                if (isReserved)
                {
                    return QrIdentifier.RFU;
                }

                return identifier;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// กำหนดค่าพื้นฐานของ QR Data Object
        /// </summary>
        /// <param name="rawValue">ข้อมูลดิบ</param>
        public QrDataObject(string rawValue)
        {
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawValue)
                && rawValue.Length >= emv.MinSegmentLength;
            if (!isArgumentValid)
            {
                throw new ArgumentException("Content must has a minimum length of 5 characters.");
            }

            RawValue = rawValue;
            Id = getIdSegment();
            Length = getLength();
            Value = getValue();

            string getIdSegment()
            {
                const int IdIndex = 0;
                const int ContentLength = 2;
                return RawValue.Substring(IdIndex, ContentLength);
            }
            string getLength()
            {
                const int LengthIndex = 2;
                const int ContentLength = 2;
                return RawValue.Substring(LengthIndex, ContentLength);
            }
            string getValue()
            {
                const int ValueIndex = 4;
                return RawValue.Substring(ValueIndex);
            }
        }

        #endregion Constructors
    }
}
