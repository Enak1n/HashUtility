using System.Security.Cryptography;
using System.Text;

namespace HashUtility
{
    public static class PasswordHasher
    {
        public static string Hash(string password, string salt, bool is3DES)
        {
            var hashedSalt = RandomNumberGenerator.GetBytes(salt.Length);

            var hash = Rfc2898DeriveBytes.Pbkdf2(password, hashedSalt, 10000, HashAlgorithmName.SHA256, 256 / 8);

            if (is3DES)
            {
                var tripleDeshHash = GenerateTripleDesHash(hash);
                return string.Join(':', Convert.ToBase64String(hashedSalt), Convert.ToBase64String(tripleDeshHash));
            }

            return string.Join(':', Convert.ToBase64String(hashedSalt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(':');
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, 10000, HashAlgorithmName.SHA256, 256 / 8);
            var expectedTripleDesHash = GenerateTripleDesHash(hashInput);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput) ||
                    CryptographicOperations.FixedTimeEquals(hash, expectedTripleDesHash);
        }

        private static byte[] GenerateTripleDesHash(byte[] data)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes("put_your_encryption_key_here"));

            using (var tripleDes = new TripleDESCryptoServiceProvider())
            {
                tripleDes.Key = keyArray;
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;

                using (var encryptor = tripleDes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
    }
}
