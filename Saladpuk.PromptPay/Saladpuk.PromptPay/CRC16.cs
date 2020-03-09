using Saladpuk.Contracts;
using System.Text;

namespace Saladpuk.PromptPay
{
    public class CRC16 : ICyclicRedundancyCheck
    {
        private readonly ushort[] table = new ushort[256];

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
    }
}
