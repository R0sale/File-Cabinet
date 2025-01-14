using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class ComplementaryFunctions
    {
        public static void WriteFixedString(FileStream fileStream, string? value, int length)
        {
            if (fileStream is not null)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value ?? string.Empty);

                byte[] padded = new byte[length];
                Array.Copy(buffer, padded, Math.Min(buffer.Length, length));

                fileStream.Write(padded, 0, length);
            }
        }

        public static void WriteFixedDecimal(FileStream fileStream, decimal value, int length)
        {
            if (fileStream is not null)
            {
                int[] bits = decimal.GetBits(value);

                byte[] array = new byte[length];
                Buffer.BlockCopy(bits, 0, array, 0, length);

                fileStream.Write(array, 0, length);
            }
        }
    }
}
