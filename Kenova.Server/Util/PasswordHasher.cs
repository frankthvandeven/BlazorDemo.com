using System;
using System.Linq;
using System.Security.Cryptography;

// https://medium.com/dealeron-dev/storing-passwords-in-net-core-3de29a3da4d2

namespace Kenova.Server.Util
{
    /// <summary>
    /// Create a salted password hash.
    /// </summary>
    public class PasswordHasher
    {
        private const int _saltSize = 16; // 128 bit 
        private const int _keySize = 32; // 256 bit
        public const int _iterations = 10000;

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, _saltSize, _iterations, HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(_keySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{_iterations}.{salt}.{key}";
            }
        }

        public bool NeedsUpgrade { get; private set; } = false;

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                  "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            this.NeedsUpgrade = iterations != _iterations;

            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(_keySize);

                var verified = keyToCheck.SequenceEqual(key);

                return verified;
            }
        }
    }
}
