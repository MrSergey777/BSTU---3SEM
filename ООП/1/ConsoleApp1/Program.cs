using System;
class Program { 
static void Main()
{
    //b
    byte a = 20;
    int A = a;
    int b = 200000;
    long B = (long)b;
    bool flag = Convert.ToBoolean(1);
    char ch = Convert.ToChar(65);
    //c0
    long c = 42;
    object objB = c;
    long unboxedB = (long)objB;
    Console.WriteLine($"{unboxedB}");
    //d
    var aa= 34;
    Console.WriteLine(aa);
        //e
        int? maybeNumber = null;
        Console.WriteLine($"HasValue: {maybeNumber.HasValue}");          // False
        Console.WriteLine($"GetValueOrDefault: {maybeNumber.GetValueOrDefault()}"); // 0

    }
}