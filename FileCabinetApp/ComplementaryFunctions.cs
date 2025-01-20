using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// A class for additional functions that are needed in another classes.
    /// </summary>
    public static class ComplementaryFunctions
    {
        /// <summary>
        /// Is writing the fixed string into the file.
        /// </summary>
        /// <param name="fileStream">Filestream.</param>
        /// <param name="value">The string that we need to write.</param>
        /// <param name="length">The length of the memory that we need to provide.</param>
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

        /// <summary>
        /// Is writing the fixed decimal.
        /// </summary>
        /// <param name="fileStream">Filestream.</param>
        /// <param name="value">The decimal that we need to write.</param>
        /// <param name="length">The number of memory we need to provide.</param>
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

        /// <summary>
        /// Is reading the string.
        /// </summary>
        /// <param name="reader">Reader.</param>
        /// <param name="length">The number of memory we need to provide.</param>
        /// <returns>The string.</returns>
        public static string ReadFixedString(BinaryReader reader, int length)
        {
            try
            {
                if (reader is null)
                {
                    throw new ArgumentException("The reader is null");
                }

                char[] chars = reader.ReadChars(length);
                return new string(chars).TrimEnd('\0');
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }
    }
}
