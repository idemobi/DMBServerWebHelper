using System.Security.Cryptography;
using System.Text;

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides deterministic security hash helpers for web infrastructure code.
    /// </summary>
    public static class SecurityHashTools
    {
        /// <summary>
        ///     Generates a lowercase hexadecimal SHA-256 hash for a string value.
        /// </summary>
        /// <param name="value">
        ///     The value to hash. A <see langword="null"/> value is treated as an empty string.
        /// </param>
        /// <returns>
        ///     The SHA-256 hash encoded as lowercase hexadecimal text.
        /// </returns>
        public static string GenerateSha256(string? value)
        {
            string input = value ?? string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = SHA256.HashData(bytes);
            StringBuilder sb = new(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
