using System;
using System.Text;
using System.Numerics;
using UnityEngine;

namespace Dojo.Starknet
{
    public static class ShortString
    {
        public static FieldElement EncodeShortString(string input)
        {
            if (input.Length > 31)
                throw new ArgumentException("ShortString can only contain up to 31 ASCII characters.");

            byte[] bytes = Encoding.ASCII.GetBytes(input);
            BigInteger bigInt = new BigInteger(bytes, isUnsigned: true, isBigEndian: false);
            return new FieldElement(bigInt);
        }

        public static string DecodeShortString(FieldElement felt)
        {
            Span<byte> span = felt.Inner.data;

            string spanHex = BitConverter.ToString(span.ToArray()).Replace("-", " ");

            StringBuilder spanAscii = new StringBuilder(span.Length);
            for (int i = 0; i < span.Length; i++)
            {
                spanAscii.Append(span[i] >= 32 && span[i] <= 126 ? (char)span[i] : '.');
            }

            // Find the start and end of the actual string content
            int start = 0;
            int end = span.Length - 1;

            while (start < span.Length && span[start] == 0) start++;
            while (end >= 0 && span[end] == 0) end--;

            if (start > end)
            {
                return string.Empty;
            }

            string result = Encoding.ASCII.GetString(span.Slice(start, end - start + 1).ToArray()).TrimEnd('\0');

            return result;
        }
    }
}