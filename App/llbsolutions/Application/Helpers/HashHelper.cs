using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    internal static class HashHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        }

        public static bool VerifyHash(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

    }
}
