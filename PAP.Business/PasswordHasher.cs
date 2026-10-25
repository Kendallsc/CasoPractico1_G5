using System;
using System.Security.Cryptography;

namespace PAP.Business
{
    /// <summary>
    /// Genera y verifica hashes PBKDF2 (formato: PBKDF2$iteraciones$salt$hash, todo en Base64).
    /// Cabe de sobra en las columnas PasswordHash NVARCHAR(255).
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static string Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Derive(password, salt, Iterations);
            return string.Format("PBKDF2${0}${1}${2}", Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string stored)
        {
            if (password == null || string.IsNullOrEmpty(stored))
                return false;

            string[] parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != "PBKDF2")
                return false;

            int iterations;
            if (!int.TryParse(parts[1], out iterations))
                return false;

            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expected = Convert.FromBase64String(parts[3]);
            byte[] actual = Derive(password, salt, iterations);

            int diff = expected.Length ^ actual.Length;
            for (int i = 0; i < expected.Length && i < actual.Length; i++)
                diff |= expected[i] ^ actual[i];
            return diff == 0;
        }

        private static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
    }
}
