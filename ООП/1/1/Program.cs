// 1 
using System;

namespace MyApp
{
    public delegate void MessageDelegate(string message);

    class Program
    {
        static void Main()
        {
            int a = 9;
            bool b = true;
            uint c = 95;
            long d = -10000000;
            ulong e = 10000000;
            byte f = 100;
            sbyte g = -128;
            short h = -100;
            ushort i = 100;
            object o = null;
            string str = "C#";

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(d);
            Console.WriteLine(e);
            Console.WriteLine(f);
            Console.WriteLine(g);
            Console.WriteLine(h);
            Console.WriteLine(i);
            Console.WriteLine(o);
            Console.WriteLine(str);
            Console.WriteLine("Ввelите строку");
            string s = Console.ReadLine();
        }
    }
    
}
