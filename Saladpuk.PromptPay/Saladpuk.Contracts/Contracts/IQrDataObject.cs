using Saladpuk.Contracts.EMVCo;

namespace Saladpuk.Contracts
{
    /// <summary>
    /// มาตรฐานตัวเก็บข้อมูล QR Data Object
    /// </summary>
    public interface IQrDataObject
    {
        /// <summary>
        /// ข้อมูลดิบ
        /// </summary>
        string RawValue { get; }

        /// <summary>
        /// รหัสประเภทข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        string Id { get; }

        /// <summary>
        /// ความยาวของข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        string Length { get; }

        /// <summary>
        /// ข้อมูลที่อ่านจากข้อมูลดิบ
        /// </summary>
        string Value { get; }

        /// <summary>
        /// รหัสประเภทข้อมูลตามมาตรฐาน
        /// </summary>
        QrIdentifier IdByConvention { get; }
    }
}
