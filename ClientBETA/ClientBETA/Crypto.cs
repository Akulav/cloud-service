using System;
using System.Security.Cryptography;
using System.Text;

namespace ClientBETA
{
    class Crypto
    {
        public static string GenerateHash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            string data = BitConverter.ToString(hash);
            return data;
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
