namespace Saladpuk.Contracts
{
    /// <summary>
    /// มาตรฐานในการอ่านข้อมูลจาก QR code
    /// </summary>
    public interface IQrReader
    {
        /// <summary>
        /// แปลความหมายของข้อความให้เป็ QR code
        /// </summary>
        /// <param name="code">รหัส QR code ที่ต้องการอ่าน</param>
        /// <returns>รายละเอียดของ QR</returns>
        IQrInfo Read(string code);
    }
}
