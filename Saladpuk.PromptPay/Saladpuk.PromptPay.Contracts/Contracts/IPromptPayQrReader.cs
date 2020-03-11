using Saladpuk.EMVCo.Contracts;

namespace Saladpuk.PromptPay.Contracts
{
    /// <summary>
    /// มาตรฐานในการอ่านข้อมูลจาก QR PromptPay
    /// </summary>
    public interface IPromptPayQrReader : IQrReader
    {
        /// <summary>
        /// แปลความหมายของข้อความให้อยู่ในรูปแบบ QR PromptPay
        /// </summary>
        /// <param name="code">รหัส QR code ที่ต้องการอ่าน</param>
        /// <returns>รายละเอียดของ QR</returns>
        IPromptPayQrInfo ReadQrPromptPay(string code);
    }
}
