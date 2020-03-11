using Saladpuk.EMVCo.Contracts;
using Saladpuk.EMVCo.Models;
using Saladpuk.PromptPay.Contracts.Models;
using System;
using System.Collections.Generic;
using emv = Saladpuk.EMVCo.Contracts.EMVCoCodeConventions;

namespace Saladpuk.PromptPay.Models
{
    /// <summary>
    /// รายละเอียดของ QR
    /// </summary>
    public class PromptPayQrInfo : QrInfo
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
        public PromptPayQrInfo(List<IQrDataObject> segments = null)
            : base(segments) { }

        #endregion Constructors
    }
}
