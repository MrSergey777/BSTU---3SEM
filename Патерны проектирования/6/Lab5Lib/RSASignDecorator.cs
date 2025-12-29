using System.Security.Cryptography;
using System.Text;

namespace Lab5Lib
{
    public class RSASignDecorator : Decorator
    {
        private readonly RSA rsa;
        private readonly HashAlgorithmName hashAlgorithmName;

        public RSASignDecorator(IWriter writer, HashAlgorithmName hashAlgorithm)
            : base(writer)
        {
            this.rsa = RSA.Create(2048);
            this.hashAlgorithmName = hashAlgorithm;
        }

        public override string? Save(string? message)
        {
            if (message == null)
                return null;

            var parts = message.Split(Constant.Token);
            if (parts.Length < 2) // некорректный формат
                return null;

            string text = parts[0]; // исходное сообщение

            // вычисляем бинарный хеш исходного сообщения
            byte[] hashBytes = hashAlgorithmName.Name switch
            {
                "MD5" => MD5.HashData(Encoding.UTF8.GetBytes(text)),
                "SHA512" => SHA512.HashData(Encoding.UTF8.GetBytes(text)),
                _ => throw new NotSupportedException(
                    $"Алгоритм {hashAlgorithmName} не поддерживается"
                ),
            };

            // создаём подпись на хеше
            byte[] signature = rsa.SignData(
                hashBytes,
                hashAlgorithmName,
                // HashAlgorithmName.SHA512,
                RSASignaturePadding.Pkcs1
            );

            string signatureBase64 = Convert.ToBase64String(signature);
            string publicKeyBase64 = Convert.ToBase64String(rsa.ExportRSAPublicKey());

            return base.Save(
                $"{text}{Constant.Token}{signatureBase64}{Constant.Token}{publicKeyBase64}"
            );
        }
    }
}
