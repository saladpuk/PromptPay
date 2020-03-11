using Saladpuk.EMVCo.Contracts;
using Saladpuk.EMVCo.Models;
using System;
using System.Collections.Generic;

namespace Saladpuk.PromptPay
{
    /// <summary>
    /// ตัวอ่านข้อมูลจาก QR code
    /// </summary>
    public class QrReader : IQrReader
    {
        #region Fields

        /// <summary>
        /// ส่วนประกอบทั้งหมดของ QR code
        /// </summary>
        protected List<IQrDataObject> Segments = new List<IQrDataObject>();

        #endregion Fields

        #region IQrReader members

        /// <summary>
        /// แปลความหมายของข้อความให้เป็ QR code
        /// </summary>
        /// <param name="code">รหัส QR code ที่ต้องการอ่าน</param>
        /// <returns>รายละเอียดของ QR</returns>
        public IQrInfo Read(string code)
        {
            ExtractText(code);
            return CreateQrInfo();
        }

        #endregion IQrReader members

        #region Methods

        /// <summary>
        /// สร้างรายละเอียดของ QR
        /// </summary>
        /// <returns>รายละเอียดของ QR</returns>
        protected virtual IQrInfo CreateQrInfo()
            => new QrInfo(Segments);
        protected virtual void ExtractText(string text)
        {
            var reader = new QrDataObject(text);
            var (targetSegment, other) = ExtractSegments(reader);
            Segments.Add(targetSegment);
            if (!string.IsNullOrWhiteSpace(other))
            {
                ExtractText(other);
            }
        }
        protected virtual (IQrDataObject, string) ExtractSegments(IQrDataObject data)
        {
            if (!int.TryParse(data.Length, out int length))
            {
                throw new ArgumentException("LengthCode isn't valid.");
            }
            var currentValue = $"{data.Id}{data.Length}{data.Value.Substring(0, length)}";
            var firstSegment = new QrDataObject(currentValue);
            var other = data.RawValue.Substring(currentValue.Length);
            return (firstSegment, other);
        }

        #endregion Methods
    }
}
