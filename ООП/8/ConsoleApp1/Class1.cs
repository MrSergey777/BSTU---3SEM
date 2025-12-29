using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp1
{
    public delegate double Q(int s);
    public class Ev
    {
        public event Q A;
        public void em(int s) 
            {
            this.A.Invoke(s);
            } 
    }
    public static class Progaram
    {
        public static void Main()
        {
            Ev ev = new Ev();
            ev.A += (q) => {  return (double)q  ; };
            ev.em(10);
        }
    }
}
