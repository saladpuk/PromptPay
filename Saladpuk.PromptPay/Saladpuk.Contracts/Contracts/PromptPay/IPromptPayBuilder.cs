using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;

namespace Saladpuk.Contracts.PromptPay
{
    /// <summary>
    /// มาตรฐานตัวสร้าง QR PromptPay
    /// </summary>
    public interface IPromptPayBuilder : IEMVCoBuilder<IPromptPayBuilder>
    {
        #region Credit transfer

        /// <summary>
        /// สร้างรหัส QR สำหรับโอนเงินผ่านบริการพร้อมเพย
        /// </summary>
        string CreateCreditTransferQrCode();
        /// <summary>
        /// สร้างรหัส QR สำหรับโอนเงินผ่านบริการพร้อมเพย
        /// </summary>
        /// <param name="transfer">ข้อมูลการโอนเงินผ่านบริการพร้อมเพย</param>
        string CreateCreditTransferQrCode(CreditTransfer transfer);
        /// <summary>
        /// กำหนดเบอร์มือถือของผู้รับเงิน (Credit Transfer)
        /// </summary>
        /// <param name="value">เบอร์มือถือของผู้รับเงิน</param>
        IPromptPayBuilder MobileNumber(string value);
        /// <summary>
        /// กำหนดรหัสกระเป๋าเงินอิเล็กทรอนิกส์ (Credit Transfer)
        /// </summary>
        /// <param name="value">รหัสกระเป๋าเงินอิเล็กทรอนิกส์</param>
        IPromptPayBuilder EWallet(string value);
        /// <summary>
        /// กำหนดรหัสบัญชีธนาคารผู้รับเงิน (Credit Transfer)
        /// </summary>
        /// <param name="value">รหัสบัญชีธนาคารผู้รับเงิน</param>
        IPromptPayBuilder BankAccount(string value);
        /// <summary>
        /// กำหนดค่า OTA (Credit Transfer)
        /// </summary>
        /// <param name="value">ค่า OTA</param>
        IPromptPayBuilder OTA(string value);
        /// <summary>
        /// กำหนดให้เป็นรูปแบบร้านเป็นผู้แสดง QR ให้ลูกค้าสแกน (Credit Transfer)
        /// </summary>
        IPromptPayBuilder MerchantPresentedQR();
        /// <summary>
        /// กำหนดให้เป็นรูปแบบลูกค้าเป็นผู้แสดง QR Code ให้ร้านค้าสแกน (Credit Transfer)
        /// </summary>
        IPromptPayBuilder CustomerPresentedQR();

        #endregion Credit transfer

        #region Biller

        /// <summary>
        /// สร้างรหัส QR ขอเรียกเก็บเงินสำหรับผู้ประกอบการ
        /// </summary>
        string GetBillPaymentQrCode();
        /// <summary>
        /// สร้างรหัส QR ขอเรียกเก็บเงินสำหรับผู้ประกอบการ
        /// </summary>
        /// <param name="payment">ข้อมูลการขอเรียกเก็บเงินสำหรับผู้ประกอบการ</param>
        string GetBillPaymentQrCode(BillPayment payment);
        /// <summary>
        /// กำหนดเลขผู้ให้บริการที่เป็นตัวแทนในการรับชำระเงิน (Bill Payment)
        /// </summary>
        /// <param name="value">เลขผู้ให้บริการที่เป็นตัวแทนในการรับชำระเงิน</param>
        IPromptPayBuilder BillerSuffix(string value);
        /// <summary>
        /// กำหนดรหัสอ้างอิง 1 (Bill Payment)
        /// </summary>
        /// <param name="value">รหัสอ้างอิง 1</param>
        IPromptPayBuilder BillRef1(string value);
        /// <summary>
        /// กำหนดรหัสอ้างอิง 2 (Bill Payment)
        /// </summary>
        /// <param name="value">รหัสอ้างอิง 2</param>
        IPromptPayBuilder BillRef2(string value);
        /// <summary>
        /// กำหนดให้เป็นการใช้ภายในประเทศ (Bill Payment)
        /// </summary>
        IPromptPayBuilder DomesticMerchant();
        /// <summary>
        /// กำหนดให้เป็นการใช้ข้ามประเทศ (Bill Payment)
        /// </summary>
        IPromptPayBuilder CrossBorderMerchant();

        #endregion Biller

        /// <summary>
        /// กำหนดรหัสประจำตัวประชาชน หรือ เลขทะเบียนนิติบุคคล
        /// </summary>
        /// <param name="value">รหัสประจำตัวประชาชน หรือ เลขทะเบียนนิติบุคคล</param>
        IPromptPayBuilder NationalId(string value);
        /// <summary>
        /// กำหนดเลขประจำตัวผู้เสียภาษี หรือ เลขประจำตัวผู้เสียภาษีของร้านค้า
        /// </summary>
        /// <param name="value">เลขประจำตัวผู้เสียภาษี หรือ เลขประจำตัวผู้เสียภาษีของร้านค้า</param>
        IPromptPayBuilder TaxId(string value);
        /// <summary>
        /// จำนวนเงินที่จะเรียกเก็บ
        /// </summary>
        /// <param name="amount">จำนวนเงิน</param>
        IPromptPayBuilder Amount(double amount);
    }
}
