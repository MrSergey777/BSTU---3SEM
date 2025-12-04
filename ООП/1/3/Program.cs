using System.Text;

class Program
{
    static void Main()
    {
        //a
        string s = "A";
        string ss = "B";
        int a = s.CompareTo(ss);
        Console.WriteLine(a);
        //b
        // Создаём три строки разными способами
        string s1 = "Hello";
        string s2 = new string(new char[] { 'W', 'o', 'r', 'l', 'd' });
        char[] exclam = { '!', '!' };
        string s3 = new string(exclam);

        // 1. Сцепление
        string concat = s1 + " " + s2 + s3;
        Console.WriteLine(concat);    // Hello World!!           

        // 2. Копирование
        string copy = String.Copy(concat);
        Console.WriteLine(copy);      // Hello World!!           

        // 3. Выделение подстроки
        string sub = concat.Substring(6, 5);
        Console.WriteLine(sub);       // World                    

        // 4. Разбиение на слова
        string[] words = concat.Split(' ');
        Console.WriteLine(string.Join("|", words)); // Hello|World!!    

        // 5. Вставка подстроки
        string inserted = concat.Insert(5, ",");
        Console.WriteLine(inserted);   // Hello, World!!          

        // 6. Удаление подстроки
        string removed = inserted.Remove(5, 1);
        Console.WriteLine(removed);    // Hello World!!           

        // Интерполяция строк
        int count = words.Length;
        string interp = $"В строке {count} слова: {string.Join(", ", words)}";
        Console.WriteLine(interp);      // В строке 2 слова: Hello, World!!
           //c
        string empty = "";        // пустая строка
        string nullStr = null;    // null-строка

        // Проверка на null или пустоту
        Console.WriteLine(string.IsNullOrEmpty(empty));   // True
        Console.WriteLine(string.IsNullOrEmpty(nullStr)); // True
        //d
        var sb = new StringBuilder("Hello, World!");

        sb.Remove(5, 2);           // удалили ", "
        sb.Insert(0, "Start-");    // вставили в начало
        sb.Append("-End");          // добавили в конец

        Console.WriteLine(sb);      // Start-HelloWorld!-End

    }
}
