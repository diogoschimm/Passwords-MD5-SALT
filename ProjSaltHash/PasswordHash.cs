using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjSaltHash
{
    public class PasswordHash 
    {
        private const int SALT_LENGTH = 19;

        public SaltHash GetHash(string password)
        {
            var salt = CreateSalt();
            return GetHash(password, salt);
        }

        public SaltHash GetHash(string password, string salt)
        {
            var provider = MD5.Create();
            var bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password));
            var hash = Base64.Encode(salt + FormatHash(bytes));

            return new SaltHash(salt, hash); 
        }

        public string GetSalt(string passwordHashedBase64)
        {
            var data = Base64.Decode(passwordHashedBase64);
            var salt = data.Substring(0, SALT_LENGTH * 2);
            return salt;
        }

        public bool CompareHashs(string passwordHashedBase64, string password)
        {
            var salt = GetSalt(passwordHashedBase64);
            var hash = GetHash(password, salt).Hash;

            return passwordHashedBase64.Equals(hash);
        }

        private string CreateSalt()
        {
            byte[] buff = new byte[SALT_LENGTH];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);

            var salt = FormatHash(buff);

            return salt;
        }

        private string FormatHash(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "");
        } 
    }
}
