using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
namespace ne
{
    public delegate double A(string a);
    public class lab
    {
        public event A a;
        public void Eve(string a)
        {
            this.a.Invoke(a);
        }
    }
    public class Program
    {
        public static void Main()
        {
            lab lab = new lab();
            lab.a += (x) => { Console.WriteLine(Convert.ToDouble(x)); return Convert.ToDouble(x); };
            lab.Eve("1234");
        }
    }
}
