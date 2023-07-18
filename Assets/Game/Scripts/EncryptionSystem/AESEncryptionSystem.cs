using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Game.Encryption
{
    public class AESEncryptionSystem : IEncryptionSystem
    {
        private byte[] key;
        private byte[] iv;

        public AESEncryptionSystem()
        {
            if (PlayerPrefs.HasKey("encryption_key"))
            {
                key = System.Text.Encoding.ASCII.GetBytes(PlayerPrefs.GetString("encryption_key"));
                iv = System.Text.Encoding.ASCII.GetBytes(PlayerPrefs.GetString("encryption_iv"));
            }
            else
            {
                using (AesManaged aes = new AesManaged())
                {
                    key = aes.Key;
                    iv = aes.IV;

                    PlayerPrefs.SetString("encryption_key", System.Text.Encoding.ASCII.GetString(key));
                    PlayerPrefs.SetString("encryption_iv", System.Text.Encoding.ASCII.GetString(iv));
                }
            }
        }

        public string Decrypt(byte[] cipherText)
        {
            string plaintext = null;
            // Create AesManaged
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        public byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            // Create a new AesManaged.
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                // Create MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream
                    // to encrypt
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }

            return encrypted;
        }
    }
}
