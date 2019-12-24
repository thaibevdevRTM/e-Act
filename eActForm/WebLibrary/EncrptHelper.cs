using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections;

namespace WebLibrary
{
    public static class EncrptHelper
    {




        #region RSA
        public static string EncryptString(string inputString, int dwKeySize,
                             string xmlString)
        {
            // TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider =
                                          new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int keySize = dwKeySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            // The hash function in use by the .NET RSACryptoServiceProvider here 
            // is SHA1
            // int maxLength = ( keySize ) - 2 - 
            //              ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[
                        (dataLength - maxLength * i > maxLength) ? maxLength :
                                                      dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0,
                                  tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes,
                                                                          true);
                // Be aware the RSACryptoServiceProvider reverses the order of 
                // encrypted bytes. It does this after encryption and before 
                // decryption. If you do not require compatibility with Microsoft 
                // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                // next line and the corresponding one in the DecryptString function.
                Array.Reverse(encryptedBytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only 
                // ASCII characters
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        public static string DecryptString(string inputString, int dwKeySize,
                                     string xmlString)
        {
            // TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider
                                     = new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ?
              (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(
                     inputString.Substring(base64BlockSize * i, base64BlockSize));
                // Be aware the RSACryptoServiceProvider reverses the order of 
                // encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic 
                // API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the 
                // EncryptString function.
                Array.Reverse(encryptedBytes);
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(
                                    encryptedBytes, true));
            }
            return Encoding.UTF32.GetString(arrayList.ToArray(
                                      Type.GetType("System.Byte")) as byte[]);
        }
        #endregion

        #region Use EndCoding
        public static string EncodingId(string Id)
        {
            Encoding Encode = Encoding.GetEncoding("iso8859-1");

            Encoding En = Encoding.Unicode;
            byte[] code = Encode.GetBytes(Id);
            return En.GetString(code).ToString();

        }
        public static string DecodingId(string Id)
        {
            Encoding Encode = Encoding.GetEncoding("iso8859-1");
            Encoding En = Encoding.Unicode;
            byte[] code = En.GetBytes(Id);
            return Encode.GetString(code).ToString();
        }
        #endregion

        #region Use TripleDES
        //Use TripleDES Algolithm
        public static string TDSEncryp(string strVar)
        {
            MemoryStream outPut = new MemoryStream();
            byte[] byteData = new UnicodeEncoding().GetBytes(strVar);
            TripleDESCryptoServiceProvider Tdes = new TripleDESCryptoServiceProvider();
            byte[] Keys = {0x01,0x01,0x01,0x02,0x03,0x05, 
							  0x08,0x09,0x10,0x11,0x12,0x13, 
							  0x01,0x01,0x01,0x02,0x03,0x04, 
							  0x08,0x09,0x10,0x11,0x12,0x13};
            byte[] iv = { 0x005, 0x001, 0x0011, 0x001, 0x005, 0x001, 0x024, 0x001 };
            Tdes.Key = Keys; Tdes.IV = iv;
            CryptoStream Crypto = new CryptoStream(outPut, Tdes.CreateEncryptor(), CryptoStreamMode.Write);
            Crypto.Write(byteData, 0, byteData.Length);
            Crypto.Close();
            outPut.Close();
            return new UnicodeEncoding().GetString(outPut.ToArray());
        }
        public static string TDSDecryp(string DecrypVar)
        {
            MemoryStream outPut = new MemoryStream();
            byte[] byteData = new UnicodeEncoding().GetBytes(DecrypVar);
            TripleDESCryptoServiceProvider Tdes = new TripleDESCryptoServiceProvider();
            byte[] Keys = {0x01,0x01,0x01,0x02,0x03,0x05, 
							  0x08,0x09,0x10,0x11,0x12,0x13, 
							  0x01,0x01,0x01,0x02,0x03,0x04, 
							  0x08,0x09,0x10,0x11,0x12,0x13};
            byte[] iv = { 0x005, 0x001, 0x0011, 0x001, 0x005, 0x001, 0x024, 0x001 };
            Tdes.Key = Keys; Tdes.IV = iv;
            CryptoStream Crypto = new CryptoStream(outPut, Tdes.CreateDecryptor(), CryptoStreamMode.Write);
            Crypto.Write(byteData, 0, byteData.Length);
            return new UnicodeEncoding().GetString(outPut.ToArray());
        }
        #endregion

        #region MD5 Crypto
        //private static string key = "sfdjf48mdfdf3054";//key forMD5
        private static string key = "123456789";
        public static string MD5Encryp(string plainText)
        {
            string encrypted = null;
            try
            {
                byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(plainText);
                byte[] pwdhash = null;
                MD5CryptoServiceProvider hashmd5;

                //generate an MD5 hash from the password. 
                //a hash is a one way encryption meaning once you generate
                //the hash, you cant derive the password back from it.
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                hashmd5 = null;

                // Create a new TripleDES service provider 
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = CipherMode.ECB;

                encrypted = Convert.ToBase64String(
                    tdesProvider.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string str = e.Message;
                throw;
            }
            return encrypted;
        }

        public static String MD5Decryp(string encryptedString)
        {
            string decyprted = null;
            byte[] inputBytes = null;

            try
            {
                inputBytes = Convert.FromBase64String(encryptedString);
                byte[] pwdhash = null;
                MD5CryptoServiceProvider hashmd5;

                //generate an MD5 hash from the password. 
                //a hash is a one way encryption meaning once you generate
                //the hash, you cant derive the password back from it.
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                hashmd5 = null;

                // Create a new TripleDES service provider 
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = CipherMode.ECB;

                decyprted = ASCIIEncoding.ASCII.GetString(
                    tdesProvider.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string str = e.Message;
                throw;
            }
            return decyprted;
        }
        #endregion
    }
}
