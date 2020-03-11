using Saladpuk.EMVCo.Contracts;
using System.Text;

namespace Saladpuk.EMVCo
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของข้อมูล
    /// </summary>
    public class CRC16 : ICyclicRedundancyCheck
    {
        #region Fields
        
        private readonly ushort[] table = new ushort[256];

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับตัวตรวจสอบความถูกต้องของข้อมูล
        /// </summary>
        public CRC16()
        {
            const ushort polynomial = 0x1021;
            ushort temp, a;
            for (var i = 0; i < table.Length; i++)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (var j = 0; j < 8; j++)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ polynomial);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// สร้างรหัสเช็คความถูกต้องของข้อมูล
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการนำไปสร้างรหัส</param>
        public string ComputeChecksum(string data)
        {
            ushort crc = 0xffff;
            var bytes = Encoding.ASCII.GetBytes(data);
            for (var i = 0; i < bytes.Length; i++)
            {
                crc = (ushort)((crc << 8) ^ table[(crc >> 8) ^ (0xff & bytes[i])]);
            }
            return crc.ToString("X").PadLeft(4, '0');
        }

        #endregion Methods
    }
}
