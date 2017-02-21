using System;
using System.Security.Cryptography;
using System.Text;

namespace Our.Umbraco.AuthU.Extensions
{
    public static class StringExtensions
    {
        internal static string GenerateHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        internal static string Encrypt(this string input, string key)
        {
            var inputArray = Encoding.UTF8.GetBytes(input);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tripleDES.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        internal static string Decrypt(this string input, string key)
        {
            var inputArray = Convert.FromBase64String(input);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tripleDES.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
