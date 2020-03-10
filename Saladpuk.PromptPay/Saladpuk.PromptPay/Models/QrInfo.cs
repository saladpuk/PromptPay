using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using emv = Saladpuk.Contracts.EMVCo.EMVCoCodeConventions;

namespace Saladpuk.PromptPay.Models
{
    /// <summary>
    /// รายละเอียดของ QR
    /// </summary>
    public class QrInfo : IQrInfo
    {
        #region Properties

        /// <summary>
        /// QR นี้สามารถจ่ายเงินซ้ำได้หรือไม่ ?
        /// </summary>
        public bool Reusable => (PointOfInitiationMethod ?? string.Empty) == emv.Static;

        /// <summary>
        /// สกุลเงินที่ใช้จ่าย
        /// </summary>
        public string Currency => Enum.TryParse(TransactionCurrency, out CurrencyCode currencyCode) ? currencyCode.ToString() : null;

        /// <summary>
        /// ข้อมูลการโอนเงินผ่านบริการพร้อมเพย
        /// </summary>
        public CreditTransfer CreditTransfer { get; set; }

        /// <summary>
        /// ข้อมูลการขอเรียกเก็บเงินสำหรับผู้ประกอบการ
        /// </summary>
        public BillPayment BillPayment { get; set; }

        #endregion Properties

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
        
        private string getSegment(QrIdentifier identifier)
            => Segments?.LastOrDefault(it => it.IdByConvention == identifier)?.Value ?? null;

        #endregion Methods

        #region IQRInfo members

        public IList<IQrDataObject> Segments { get; set; }
        public string PayloadFormatIndicator => getSegment(QrIdentifier.PayloadFormatIndicator);
        public string PointOfInitiationMethod => getSegment(QrIdentifier.PointOfInitiationMethod);
        public string MerchantAccountInformation => getSegment(QrIdentifier.MerchantAccountInformation);
        public string MerchantCategoryCode => getSegment(QrIdentifier.MerchantCategoryCode);
        public string TransactionCurrency => getSegment(QrIdentifier.TransactionCurrency);
        public string TransactionAmount => getSegment(QrIdentifier.TransactionAmount);
        public string TipOrConvenienceIndicator => getSegment(QrIdentifier.TipOrConvenienceIndicator);
        public string ValueOfConvenienceFeeFixed => getSegment(QrIdentifier.ValueOfConvenienceFeeFixed);
        public string ValueOfConvenienceFeePercentage => getSegment(QrIdentifier.ValueOfConvenienceFeePercentage);
        public string CountryCode => getSegment(QrIdentifier.CountryCode);
        public string MerchantName => getSegment(QrIdentifier.MerchantName);
        public string MerchantCity => getSegment(QrIdentifier.MerchantCity);
        public string PostalCode => getSegment(QrIdentifier.PostalCode);
        public string AdditionalData => getSegment(QrIdentifier.AdditionalData);
        public string CRC => getSegment(QrIdentifier.CRC);
        public string MerchantInformationLanguageTemplate => getSegment(QrIdentifier.MerchantInformationLanguageTemplate);
        public string RFU => getSegment(QrIdentifier.RFU);

        #endregion IQRInfo members
    }
}
