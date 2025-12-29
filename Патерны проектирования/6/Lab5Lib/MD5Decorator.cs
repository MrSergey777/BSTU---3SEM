using System.Security.Cryptography;
using System.Text;

namespace Lab5Lib
{
    public class MD5Decorator : Decorator
    {
        public MD5Decorator(IWriter writer)
            : base(writer) { }
        00000000
        public override string? Save(string? message)
        {
            string hash;
            using (var md5 = MD5.Create())
            {
                hash = Convert.ToHexString(md5.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }

            return base.Save($"{message}{Constant.Token}{hash}");
        }
    }
}
