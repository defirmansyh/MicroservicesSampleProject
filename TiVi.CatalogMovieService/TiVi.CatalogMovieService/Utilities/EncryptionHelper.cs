using System.Security.Cryptography;
using System.Text;

namespace TiVi.UserCatalogService.Utilities
{
    public class EncryptionHelper
    {
        private static string _key = "t1v1keys";
        private static string privatekey = "keyst1v1";

        #region Manipulate Data
        public static byte[] ComputeSha256Byte(string rawData)
        {
            using (SHA256 shA256 = SHA256.Create())
                return shA256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }
        public static string ByteToHexa(byte[] text)
        {
            StringBuilder hex = new StringBuilder(text.Length * 2);
            foreach (byte b in text)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static byte[] HexaToByte(string hexvalue)
        {
            if (hexvalue.Length % 2 != 0)
                hexvalue = "0" + hexvalue;
            int length = hexvalue.Length / 2;
            byte[] numArray = new byte[length];
            for (int index = 0; index < length; ++index)
            {
                string str = hexvalue.Substring(2 * index, 2);
                numArray[index] = Convert.ToByte(str, 16);
            }
            return numArray;
        }
        #endregion

        #region For Password
        public static string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    return "";
                }
                plainText = plainText + privatekey;
                string Return = null;
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                byte[] inputtextbyteArray = System.Text.Encoding.UTF8.GetBytes(plainText);
                using (DESCryptoServiceProvider dsp = new DESCryptoServiceProvider())
                {
                    var memstr = new MemoryStream();
                    var crystr = new CryptoStream(memstr, dsp.CreateEncryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
                    crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
                    crystr.FlushFinalBlock();
                    return Convert.ToBase64String(memstr.ToArray());
                }
                return Return;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                {
                    return "";
                }
                string x = null;
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                byte[] inputtextbyteArray = new byte[encryptedText.Replace(" ", "+").Length];
                //This technique reverses base64 encoding when it is received over the Internet.
                inputtextbyteArray = Convert.FromBase64String(encryptedText.Replace(" ", "+"));
                using (DESCryptoServiceProvider dEsp = new DESCryptoServiceProvider())
                {
                    var memstr = new MemoryStream();
                    var crystr = new CryptoStream(memstr, dEsp.CreateDecryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
                    crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
                    crystr.FlushFinalBlock();
                    var result = Encoding.UTF8.GetString(memstr.ToArray());

                    result = result.Replace(privatekey, "");

                    return result;

                }
                return x;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion


        #region For Id
        public static string EncryptId(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "";
                }
                using (AesManaged aesManaged = new AesManaged())
                {
                    byte[] key = ComputeSha256Byte(privatekey);
                    byte[] IV = new byte[16];
                    aesManaged.Mode = CipherMode.CBC;
                    aesManaged.Padding = PaddingMode.PKCS7;
                    ICryptoTransform encryptor = aesManaged.CreateEncryptor(key, IV);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                                streamWriter.Write(text);
                            return ByteToHexa(memoryStream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public static string DecryptId(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "";
                }
                using (AesManaged aesManaged = new AesManaged())
                {
                    byte[] byteText = HexaToByte(text);
                    byte[] key = ComputeSha256Byte(privatekey);
                    byte[] IV = new byte[16];
                    aesManaged.Mode = CipherMode.CBC;
                    aesManaged.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = aesManaged.CreateDecryptor(key, IV);
                    using (MemoryStream memoryStream = new MemoryStream(byteText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                                return streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        #endregion
    }
}
