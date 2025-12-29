using System.Security.Cryptography;
using System.Text;
using Lab5Lib;

namespace Lab5Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
            Test6();
            Test7();
            Test8();
            Test9();
            Test10();
            Test11();
            Test12();
            Test13();
        }

        static void Test1()
        {
            IWriter writer = new StrWriter();
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.SHA512);
            IWriter hash = new SHA512Decorator(rsa);
            string? result = hash.Save("AAAAABBBBCCCCC");

            bool testresult = TestSHA512_SA(result, Constant.Token);
            Console.WriteLine($"тест 1 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test2()
        {
            IWriter writer = new FileWriter("test2.txt");
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.SHA512);
            IWriter hash = new SHA512Decorator(rsa);
            string? result = hash.Save("BBBBCCCCC");

            string? message;
            using (var st = new StreamReader("test2.txt"))
            {
                message = st.ReadLine();
            }
            bool testresult = TestSHA512_SA(message, Constant.Token);
            Console.WriteLine($"тест 2 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test3()
        {
            IWriter writer = new StrWriter();
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.MD5);
            IWriter hash = new MD5Decorator(rsa);

            string? result = hash.Save("AAAAACCCCBBBBB");

            bool testresult = TestMD5_SA(result, Constant.Token);
            Console.WriteLine($"тест 3 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test4()
        {
            IWriter writer = new FileWriter("test4.txt");
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.MD5);
            IWriter hash = new MD5Decorator(rsa);

            string? result = hash.Save("AAAAABBBBDDDDCCCCC");

            string? message;
            using (var reader = new StreamReader("test4.txt"))
            {
                message = reader.ReadLine();
            }

            bool testresult = TestMD5_SA(message, Constant.Token);
            Console.WriteLine($"тест 4 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test5()
        {
            IWriter writer = new StrWriter();
            IWriter hash = new SHA512Decorator(writer);

            string? result = hash.Save("AAAAAAAEEEEEEBBBBBBCCCCCCC");

            bool testresult = TestSHA512(result, Constant.Token);
            Console.WriteLine($"тест 5 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test6()
        {
            IWriter writer = new StrWriter();
            IWriter hash = new MD5Decorator(writer);

            string? result = hash.Save("AAAAABBBBCCCCCHHHH");

            bool testresult = TestMD5(result, Constant.Token);
            Console.WriteLine($"тест 6 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test7()
        {
            IWriter writer = new FileWriter("test7.txt");
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.SHA512);
            IWriter hash = new SHA512Decorator(rsa);

            string? result = hash.Save("AAAAABBBBDDDDCCCCC");

            string? message;
            using (var reader = new StreamReader("test7.txt"))
            {
                message = reader.ReadLine();
            }

            bool testresult = TestSHA512_SA(message, Constant.Token);
            Console.WriteLine($"тест 7 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test8()
        {
            IWriter writer = new StrWriter();
            IWriter hash = new SHA512Decorator(writer);

            string? result = hash.Save("AAAAABBBBCCCCCT");

            bool testresult = TestMD5(result, Constant.Token);
            Console.WriteLine($"тест 8 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test9()
        {
            IWriter writer = new StrWriter();
            IWriter hash = new SHA512Decorator(writer);

            string? result = hash.Save($"{"AAAA"}{Constant.Token}{"BBBBCCCCCT"}");

            bool testresult = TestSHA512(result, Constant.Token);
            Console.WriteLine($"тест 9 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test10()
        {
            IWriter writer = new StrWriter();
            IWriter hash = new MD5Decorator(writer);

            string? result = hash.Save($"{"AAAA"}{Constant.Token}{"BBBBCCCCCT"}");

            bool testresult = TestSHA512(result, Constant.Token);
            Console.WriteLine($"тест 10 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test11()
        {
            IWriter writer = new StrWriter();
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.SHA512);
            IWriter hash = new MD5Decorator(rsa);

            string? result = hash.Save("HHHAAAAABBBBCCCCC");

            bool testresult = TestSHA512_SA(result, Constant.Token);
            Console.WriteLine($"тест 11 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test12()
        {
            IWriter writer = new FileWriter("test12.txt");
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.SHA512);
            IWriter hash = new SHA512Decorator(rsa);

            string? result = hash.Save("BBBGCCCCC");

            string? message;
            using (var reader = new StreamReader("test12.txt"))
            {
                message = reader.ReadLine();
            }

            bool testresult = TestSHA512_SA("BBBGCCCCC", Constant.Token);
            Console.WriteLine($"тест 12 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static void Test13()
        {
            IWriter writer = new FileWriter("test13.txt");
            IWriter rsa = new RSASignDecorator(writer, HashAlgorithmName.MD5);
            IWriter hash = new MD5Decorator(rsa);
            string result = hash.Save("BBBGCCCCC");

            StreamReader reader = new StreamReader("test13.txt");
            string message = reader.ReadLine();
            reader.Close();
            bool testresult = TestSHA512_SA(message, Constant.Token);
            Console.WriteLine($"тест 13 {(testresult ? "успешно" : "НЕ успешно")}");
        }

        static bool TestSHA512(string result, char Token)
        {
            if (string.IsNullOrEmpty(result))
                return false;
            var parts = result.Split(Token);
            if (parts.Length != 2)
                return false;

            using var sha = SHA512.Create();
            var hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(parts[0])));
            return parts[1] == hash;
        }

        static bool TestMD5(string result, char Token)
        {
            if (string.IsNullOrEmpty(result))
                return false;
            var parts = result.Split(Token);
            if (parts.Length != 2)
                return false;

            using var md5 = MD5.Create();
            var hash = Convert.ToHexString(md5.ComputeHash(Encoding.UTF8.GetBytes(parts[0])));
            return parts[1] == hash;
        }

        static bool TestSHA512_SA(string? result, char Token)
        {
            if (string.IsNullOrEmpty(result))
                return false;

            var parts = result.Split(Token);
            if (parts.Length != 3)
                return false; // message, signature, publicKey

            string message = parts[0]; // исходное сообщение
            string signatureBase64 = parts[1]; // подпись
            string publicKeyBase64 = parts[2]; // публичный ключ

            try
            {
                byte[] hashBytes = SHA512.HashData(Encoding.UTF8.GetBytes(message));

                using var rsa = RSA.Create();
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyBase64), out _);

                return rsa.VerifyData(
                    hashBytes,
                    Convert.FromBase64String(signatureBase64),
                    HashAlgorithmName.SHA512,
                    RSASignaturePadding.Pkcs1
                );
            }
            catch
            {
                return false;
            }
        }

        static bool TestMD5_SA(string? result, char Token)
        {
            if (string.IsNullOrEmpty(result))
                return false;

            var parts = result.Split(Token);
            if (parts.Length != 3)
                return false; // message, signature, publicKey

            string message = parts[0];
            string signatureBase64 = parts[1];
            string publicKeyBase64 = parts[2];

            try
            {
                byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(message));

                using var rsa = RSA.Create();
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyBase64), out _);

                return rsa.VerifyData(
                    hashBytes,
                    Convert.FromBase64String(signatureBase64),
                    HashAlgorithmName.MD5,
                    // HashAlgorithmName.SHA512,
                    RSASignaturePadding.Pkcs1
                );
            }
            catch
            {
                return false;
            }
        }
    }
}
