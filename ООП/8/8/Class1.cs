namespace s 
{
    public delegate char[] A(string s);
    public class Q
    {
        public event A aa;
        public void EE(string s)
        {

            this.aa.Invoke(s);
        }
    }
}
   /* public class Program
    {
        public static void Main()
        {
            Q q = new Q();
            q.aa += (s) => { Console.WriteLine(s.ToCharArray()); return s.ToCharArray(); };
            q.EE("aaaa");
        }
    }
}
*/