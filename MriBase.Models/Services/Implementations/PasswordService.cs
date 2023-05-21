using System;
using System.Security.Cryptography;
using System.Text;

namespace MriBase.Models.Services.Implementations
{
    public class PasswordService
    {
        public static string ComputeHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            using (var sha512 = SHA512.Create())
            {
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string ComputeHash(string password, out string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                var saltBytes = GenerateSalt(64);
                salt = Convert.ToBase64String(saltBytes);

                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password + ':' + salt));

                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string ComputeHash(string password, string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password + ':' + salt));

                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool CheckPassword(string password, string salt, string dbStoredPassword)
        {
            using (var sha512 = SHA512.Create())
            {
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password + ':' + salt));

                return Convert.ToBase64String(hashedBytes) == dbStoredPassword;
            }
        }

        private static byte[] GenerateSalt(int saltLength)
        {
            using (RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[saltLength];
                saltCellar.GetBytes(salt);
                return salt;
            }
        }
    }
}