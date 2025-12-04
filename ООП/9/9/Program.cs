

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SingleFileDemo
{
    // Класс Book с базовыми свойствами и реализацией IEquatable для поиска/сравнения
    public class Book : IEquatable<Book>
    {
        public string ISBN { get; set; }    // уникальный идентификатор
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public Book(string isbn, string title, string author, int year)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            Year = year;
        }

        public override string ToString() => $"{Title} by {Author} ({Year}) [ISBN: {ISBN}]";

        public bool Equals(Book other)
        {
            if (other is null) return false;
            return ISBN == other.ISBN;
        }

        public override bool Equals(object obj) => Equals(obj as Book);
        public override int GetHashCode() => ISBN?.GetHashCode() ?? 0;
    }

    // Класс Library: управляет List<Book> и IDictionary<string, Book>
    public class Library
    {
        private readonly List<Book> books = new List<Book>();
        private readonly Dictionary<string, Book> bookByIsbn = new Dictionary<string, Book>();

        // Добавление книги (в обе коллекции)
        public bool AddBook(Book b)
        {
            if (b == null || string.IsNullOrWhiteSpace(b.ISBN)) return false;
            if (bookByIsbn.ContainsKey(b.ISBN)) return false;
            books.Add(b);
            bookByIsbn[b.ISBN] = b;
            return true;
        }

        // Удаление по ISBN
        public bool RemoveByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;
            if (!bookByIsbn.TryGetValue(isbn, out var b)) return false;
            bookByIsbn.Remove(isbn);
            return books.Remove(b);
        }

        // Поиск по части заголовка
        public List<Book> FindByTitle(string titlePart)
        {
            if (string.IsNullOrWhiteSpace(titlePart)) return new List<Book>();
            return books.Where(x => x.Title.IndexOf(titlePart, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Вывод всех книг
        public void PrintAll()
        {
            Console.WriteLine("Library contents (List):");
            if (books.Count == 0) Console.WriteLine("  (пусто)");
            foreach (var b in books) Console.WriteLine("  " + b);
            Console.WriteLine("Library keys (Dictionary): " + (bookByIsbn.Count == 0 ? "(пусто)" : string.Join(", ", bookByIsbn.Keys)));
        }

        // Получить копии коллекций для внешнего использования
        public List<Book> GetList() => new List<Book>(books);
        public IDictionary<string, Book> GetDictionary() => new Dictionary<string, Book>(bookByIsbn);
    }

    // Демонстрация работы с универсальной коллекцией List<int> и копированием в Dictionary<int,int>
    public static class GenericCollectionDemo
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Generic collection demo (List<int>) ===");

            // a) Создаем и выводим List<int>
            var numbers = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80 };
            Console.WriteLine("Initial List<int>:");
            PrintList(numbers);

            // b) Удаляем n последовательных элементов (например, n=3, начиная с индекса 2)
            int startIndex = 2;
            int n = 3;
            Console.WriteLine($"\nУдаляем {n} элементов, начиная с индекса {startIndex}.");
            if (startIndex >= 0 && startIndex < numbers.Count)
            {
                int removeCount = Math.Min(n, numbers.Count - startIndex);
                numbers.RemoveRange(startIndex, removeCount);
            }
            PrintList(numbers);

            // c) Добавляем элементы всеми доступными методами для List<T>
            Console.WriteLine("\nДобавляем элементы разными способами:");
            numbers.Add(90); // Add
            numbers.AddRange(new[] { 100, 110 }); // AddRange
            numbers.Insert(1, 15); // Insert
            numbers.InsertRange(3, new[] { 25, 27 }); // InsertRange
            PrintList(numbers);

            // d) Создаем вторую коллекцию Dictionary<int,int> и заполняем данными из первой
            var dict = new Dictionary<int, int>();
            Console.WriteLine("\nКопируем данные из List<int> в Dictionary<int,int> (ключи — индексы).");
            for (int i = 0; i < numbers.Count; i++)
            {
                int key = i; // генерируем ключ как индекс
                int value = numbers[i];
                while (dict.ContainsKey(key)) key++; // на всякий случай
                dict[key] = value;
            }

            // e) Вывод второй коллекции
            Console.WriteLine("\nDictionary<int,int> contents:");
            if (dict.Count == 0) Console.WriteLine("  (пусто)");
            foreach (var kv in dict) Console.WriteLine($"  Key={kv.Key}, Value={kv.Value}");

            // f) Найти заданное значение (например, 100)
            int searchValue = 100;
            bool found = dict.Values.Contains(searchValue);
            Console.WriteLine($"\nЗначение {searchValue} найдено во второй коллекции: {found}");
            if (found)
            {
                var keys = dict.Where(kv => kv.Value == searchValue).Select(kv => kv.Key);
                Console.WriteLine("Ключи с этим значением: " + string.Join(", ", keys));
            }
        }

        private static void PrintList(List<int> list)
        {
            Console.WriteLine("List<int>: " + (list.Count == 0 ? "(пусто)" : string.Join(", ", list)));
        }
    }

    // Демонстрация ObservableCollection<Book> с обработчиком CollectionChanged
    public static class ObservableDemo
    {
        // Обработчик события CollectionChanged
        private static void OnBooksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine($"\nCollectionChanged: Action = {e.Action}");
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    Console.WriteLine("  Added: " + item);
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                    Console.WriteLine("  Removed: " + item);
            }
        }

        public static void Run()
        {
            Console.WriteLine("\n=== ObservableCollection<Book> demo ===");
            var obs = new ObservableCollection<Book>();
            obs.CollectionChanged += OnBooksChanged;

            var b1 = new Book("978-10", "Observable Patterns", "A. Author", 2021);
            var b2 = new Book("978-11", "Events in .NET", "B. Writer", 2018);

            Console.WriteLine("\nДобавляем b1:");
            obs.Add(b1);

            Console.WriteLine("\nДобавляем b2:");
            obs.Add(b2);

            Console.WriteLine("\nУдаляем b1:");
            obs.Remove(b1);

            Console.WriteLine("\nТекущее содержимое ObservableCollection:");
            if (obs.Count == 0) Console.WriteLine("  (пусто)");
            foreach (var b in obs) Console.WriteLine("  " + b);
        }
    }

    // Точка входа: демонстрация всех частей в одном файле
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Library demo ===");

            var lib = new Library();
            lib.AddBook(new Book("978-1", "C# in Depth", "Jon Skeet", 2019));
            lib.AddBook(new Book("978-2", "CLR via C#", "Jeffrey Richter", 2012));
            lib.AddBook(new Book("978-3", "Pro ASP.NET Core", "Adam Freeman", 2020));

            lib.PrintAll();

            Console.WriteLine("\nПоиск по 'C#':");
            var found = lib.FindByTitle("C#");
            if (found.Count == 0) Console.WriteLine("  (ничего не найдено)");
            foreach (var b in found) Console.WriteLine("  " + b);

            Console.WriteLine("\nУдаляем ISBN 978-2");
            lib.RemoveByIsbn("978-2");
            lib.PrintAll();

            // Запуск демонстрации универсальной коллекции
            GenericCollectionDemo.Run();

            // Запуск демонстрации ObservableCollection
            ObservableDemo.Run();

            Console.WriteLine("\n=== End ===");
            // Ждем нажатия клавиши, чтобы окно консоли не закрылось сразу (удобно при запуске из IDE)
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
