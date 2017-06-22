using System.Text;

namespace HighAvaNoDb.Utils
{
    public class EncodeHelper
    {
        public static string ByteArrayToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] StringToByteArray(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }
}
