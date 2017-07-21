using System.Security.Cryptography;
using System.Text;

namespace HighAvaNoDb.Route
{
    public class KetamaHashAlgorithm : IHashAlgorithm
    {
        private const int nTime = 1;
        public long Hash(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            md5.Clear();

            long rv = ((long)(keyBytes[3 + nTime * 4] & 0xFF) << 24)
                      | ((long)(keyBytes[2 + nTime * 4] & 0xFF) << 16)
                      | ((long)(keyBytes[1 + nTime * 4] & 0xFF) << 8)
                      | ((long)keyBytes[0 + nTime * 4] & 0xFF);

            return rv & 0xffffffffL;
        }
    }
}
