using System.Security.Cryptography;
using System.Text;

namespace ContatoInteligenteAPI.Common.Helpers
{
    public static class Encryption
    {
        private static string securityKey = Environment.GetEnvironmentVariable("CRYPTO_KEY")
            ?? throw new InvalidOperationException("Chave de criptografia não configurada.");

        public static string PasswordHasher(this string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public static bool VerifyPassword(this string password, string hashedPassword)
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return isValid;
        }

        public static string EncryptAES(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null!;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(securityKey));

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.GenerateIV();

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    byte[] cipherTextBytes;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cryptoStream))
                            {
                                writer.Write(text);
                            }
                        }
                        cipherTextBytes = memoryStream.ToArray();
                    }

                    byte[] result = new byte[aesAlg.IV.Length + cipherTextBytes.Length];
                    Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
                    Buffer.BlockCopy(cipherTextBytes, 0, result, aesAlg.IV.Length, cipherTextBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string DecryptAES(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null!;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(securityKey));

                byte[] allBytes = Convert.FromBase64String(text);
                byte[] iv = new byte[16];
                byte[] cipherTextBytes = new byte[allBytes.Length - iv.Length];

                Buffer.BlockCopy(allBytes, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(allBytes, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cryptoStream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }

}
