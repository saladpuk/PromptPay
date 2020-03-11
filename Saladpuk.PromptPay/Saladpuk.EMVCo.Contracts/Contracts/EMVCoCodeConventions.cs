using System.Collections.Generic;
using System.Linq;

namespace Saladpuk.EMVCo.Contracts
{
    /// <summary>
    /// รหัสมาตรฐานของ EMVCo
    /// </summary>
    public class EMVCoCodeConventions
    {
        /// <summary>
        /// QR ประเภทจ่ายเงินได้เพียงครั้งเดียวแล้วทิ้ง
        /// </summary>
        public const string Dynamic = "11";

        /// <summary>
        /// QR ประเภทใช้ซ้ำได้หลายครั้ง
        /// </summary>
        public const string Static = "12";

        /// <summary>
        /// Reserved for Identifies the merchant (02~51).
        /// </summary>
        public static IEnumerable<int> MerchantIdRange = Enumerable.Range(2, 50);

        /// <summary>
        /// Reserved for Future Use (65~79).
        /// </summary>
        public static IEnumerable<int> RFUIdRange = Enumerable.Range(65, 15);

        /// <summary>
        /// ความยาวขั้นต่ำของ QR data object
        /// </summary>
        public const int MinSegmentLength = 5;

        /// <summary>
        /// ความยาวของค่า CRC
        /// </summary>
        public const int CRCDigits = 4;
    }
}
