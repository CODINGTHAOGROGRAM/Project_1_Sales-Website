using System.Security.Cryptography;
using System.Text;

namespace asm.Helpers
{
    public interface IDataEncode
    {
        string EnCode(string source);
    }
    public class DataEncode : IDataEncode
    {
        public string EnCode(string source)
        {
            string hash = "";
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            return hash;

        }
    }
}
