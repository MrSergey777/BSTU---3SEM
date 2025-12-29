using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
namespace ne
{
    public delegate string A(int a);
    public class lab
    {
       public event A a;
        public void Eve(int a)
        {
            this.a.Invoke(a);
        }
    }
    public class Program
    {
        public static void Main()
        {
            lab lab = new lab();
            lab.a += (x) => { Console.WriteLine(Convert.ToString(x)); return Convert.ToString(x); };
            lab.Eve(4);
        }
    }
}
