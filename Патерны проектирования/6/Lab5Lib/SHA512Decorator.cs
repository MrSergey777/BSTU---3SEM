using System.Security.Cryptography;
using System.Text;

namespace Lab5Lib
{
    public class SHA512Decorator : Decorator
    {
        public SHA512Decorator(IWriter writer)
            : base(writer) { }

        public override string? Save(string? message)
        {
            string hash;
            using (var sha = SHA512.Create())
            {
                hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }
            return base.Save($"{message}{Constant.Token}{hash}");
        }
    }
}
