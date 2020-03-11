namespace Saladpuk.EMVCo.Contracts
{
    /// <summary>
    /// มาตรฐานตัวสร้าง QR ตามมาตรฐาน EMVCo
    /// </summary>
    /// <typeparam name="T">ตัวสร้าง QR</typeparam>
    public interface IEMVCoBuilder<T> where T : IEMVCoBuilder<T>
    {
        /// <summary>
        /// เพิ่มข้อมูลดิบ
        /// </summary>
        /// <param name="rawData"></param>
        T Add(string rawData);

        /// <summary>
        /// เพิ่มข้อมูลตามประเภทของข้อมูล
        /// </summary>
        /// <param name="identifier">ประเภทข้อมูล</param>
        /// <param name="data">ข้อมูล</param>
        T Add(QrIdentifier identifier, string data);

        /// <summary>
        /// กำหนดให้เป็น QR ประเภทใช้ซ้ำได้หลายครั้ง
        /// </summary>
        T SetStaticQR();

        /// <summary>
        /// กำหนดให้เป็น QR ประเภทจ่ายเงินได้เพียงครั้งเดียวแล้วทิ้ง
        /// </summary>
        T SetDynamicQR();

        /// <summary>
        /// กำหนดจำนวนเงินที่จะเรียกเก็บ
        /// </summary>
        /// <param name="amount">จำนวนเงินที่จะเรียกเก็บ</param>
        T SetTransactionAmount(double amount);

        /// <summary>
        /// กำหนดรหัสประเทศของร้านค้า (ตามมาตรฐาน ISO 3166)
        /// </summary>
        /// <param name="code">รหัสประเทศ</param>
        T SetCountryCode(string code);

        /// <summary>
        /// กำหนดรหัสสกุลเงินที่ใช้ในการทำธุรกรรม (ตามมาตรฐาน ISO 4217)
        /// </summary>
        /// <param name="code">รหัสสกุลเงินที่ใช้ในการทำธุรกรรม</param>
        T SetCurrencyCode(CurrencyCode code);

        /// <summary>
        /// กำหนดเวอร์ชั่นของการทำธุรกรรม
        /// </summary>
        /// <param name="version">เวอร์ชั่น</param>
        T SetPayloadFormatIndicator(string version = "01");

        /// <summary>
        /// กำหนดการตรวจสอบความถูกต้องของข้อมูล
        /// </summary>
        /// <param name="crc">ตัวตรวจสอบความถูกต้องของข้อมูล</param>
        T SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc);
    }
}
