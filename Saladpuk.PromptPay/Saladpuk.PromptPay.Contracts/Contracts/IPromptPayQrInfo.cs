using Saladpuk.PromptPay.Contracts.Models;

namespace Saladpuk.EMVCo.Contracts
{
    /// <summary>
    /// มาตรฐานรายละเอียดของ QR PromptPay
    /// </summary>
    public interface IPromptPayQrInfo : IQrInfo
    {
        /// <summary>
        /// QR นี้สามารถจ่ายเงินซ้ำได้หรือไม่ ?
        /// </summary>
        bool Reusable { get; }

        /// <summary>
        /// สกุลเงินที่ใช้จ่าย
        /// </summary>
        string Currency { get; }

        /// <summary>
        /// ข้อมูลการโอนเงินผ่านบริการพร้อมเพย
        /// </summary>
        CreditTransfer CreditTransfer { get; set; }

        /// <summary>
        /// ข้อมูลการขอเรียกเก็บเงินสำหรับผู้ประกอบการ
        /// </summary>
        BillPayment BillPayment { get; set; }
    }
}
