using System.Security.Cryptography;
using System.Text;

namespace ScopeLap.Tools
{
    public class PassString
    {
        public static string ComputeSHA512(string s)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(s));
                return Convert.ToHexString(hashValue);
            }
        }
    }
}
