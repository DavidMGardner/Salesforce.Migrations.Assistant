using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Murmur;

namespace Salesforce.Migrations.Assistant.Library.Crypto
{
    // TODO: Rework this using .net framework

    public class CryptographyProvider
    {
        private const int KeySize = 24;

        public static string Encrypt(string toEncrypt, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            TripleDESCryptoServiceProvider cryptoServiceProvider1 = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            TripleDESCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1;
            SHA256CryptoServiceProvider cryptoServiceProvider3 = new SHA256CryptoServiceProvider();
            cryptoServiceProvider2.Key = ((IEnumerable<byte>)cryptoServiceProvider3.ComputeHash(Encoding.UTF8.GetBytes(key))).Take<byte>(24).ToArray<byte>();
            cryptoServiceProvider3.Clear();
            byte[] inArray = cryptoServiceProvider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            cryptoServiceProvider2.Clear();
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }

        public static string Decrypt(string toDecrypt, string key)
        {
            byte[] inputBuffer = Convert.FromBase64String(toDecrypt);
            TripleDESCryptoServiceProvider cryptoServiceProvider1 = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            TripleDESCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1;
            SHA256CryptoServiceProvider cryptoServiceProvider3 = new SHA256CryptoServiceProvider();
            cryptoServiceProvider2.Key = ((IEnumerable<byte>)cryptoServiceProvider3.ComputeHash(Encoding.UTF8.GetBytes(key))).Take<byte>(24).ToArray<byte>();
            cryptoServiceProvider3.Clear();
            byte[] bytes = cryptoServiceProvider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            cryptoServiceProvider2.Clear();
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] ComputeMurMurHash(byte[] bytesToHash)
        {
            return MurmurHash.Create128(0U, true, AlgorithmPreference.Auto).ComputeHash(bytesToHash);
        }

        // ReSharper disable once InconsistentNaming
        public static byte[] ComputeMD5Hash(byte[] bytesToHash)
        {
            return new MD5CryptoServiceProvider().ComputeHash(bytesToHash);
        }

        public static byte[] ComputeHash(MemoryStream streamToHash)
        {
            return new MD5CryptoServiceProvider().ComputeHash((Stream)streamToHash);
        }
    }
}
