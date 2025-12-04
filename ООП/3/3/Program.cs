#nullable enable
using System;
using System.Linq;
using System.Text;

namespace OneDimArrayApp
{
    public class OneDimArray
    {
        private readonly double[] _data;

        // Вложенный объект Production (название типа Production, свойство ProductionInfo)
        public Production ProductionInfo { get; init; }

        // Вложенный класс Developer (тип Developer, свойство DeveloperInfo)
        public Developer DeveloperInfo { get; init; }

        // Конструкторы
        public OneDimArray(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _data = new double[length];
            ProductionInfo = new Production { Id = 1, Name = "DefaultOrg" };
            DeveloperInfo = new Developer("Иванов Иван Иванович", 1, "Разработка");
        }

        public OneDimArray(params double[] items)
        {
            _data = items is null ? Array.Empty<double>() : (double[])items.Clone();
            ProductionInfo = new Production { Id = 101, Name = "AcmeCorp" };
            DeveloperInfo = new Developer("Петров Петр Петрович", 42, "IT");
        }

        // Пустой конструктор для удобства
        public OneDimArray() : this(Array.Empty<double>()) { }

        // Индексатор
        public double this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public int Length => _data.Length;

        public double[] ToArray() => (double[])_data.Clone();

        // Оператор вычитания скаляра
        public static OneDimArray operator -(OneDimArray arr, double scalar)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            var res = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++) res[i] = arr[i] - scalar;
            return new OneDimArray(res) { ProductionInfo = arr.ProductionInfo, DeveloperInfo = arr.DeveloperInfo };
        }

        // Конкатенация массивов
        public static OneDimArray operator +(OneDimArray a, OneDimArray b)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            if (b is null) throw new ArgumentNullException(nameof(b));
            var res = new double[a.Length + b.Length];
            Array.Copy(a._data, 0, res, 0, a.Length);
            Array.Copy(b._data, 0, res, a.Length, b.Length);
            return new OneDimArray(res)
            {
                ProductionInfo = a.ProductionInfo,
                DeveloperInfo = a.DeveloperInfo
            };
        }

        // == и != (поэлементное сравнение и длина)
        public override bool Equals(object? obj)
        {
            if (obj is not OneDimArray other) return false;
            if (Length != other.Length) return false;
            for (int i = 0; i < Length; i++)
                if (!this[i].Equals(other[i])) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in _data) hash = hash * 31 + v.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(OneDimArray? a, OneDimArray? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(OneDimArray? a, OneDimArray? b) => !(a == b);

        // > и < используются как "проверка вхождения элемента"
        public static bool operator >(OneDimArray arr, double value)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            return arr._data.Contains(value);
        }

        public static bool operator <(OneDimArray arr, double value)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            return arr._data.Contains(value);
        }

        public override string ToString() => "[" + string.Join(", ", _data.Select(d => d.ToString("G"))) + "]";

        // Вложенный объект Production
        public class Production
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public override string ToString() => $"Production(Id={Id}, Name={Name})";
        }

        // Вложенный класс Developer
        public class Developer
        {
            public string FullName { get; set; }
            public int Id { get; set; }
            public string Department { get; set; }

            public Developer(string fullName, int id, string department)
            {
                FullName = fullName;
                Id = id;
                Department = department;
            }

            public override string ToString() => $"Developer(Id={Id}, Name={FullName}, Dept={Department})";
        }
    }

    public static class StatisticOperation
    {
        public static double Sum(OneDimArray arr)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            return arr.ToArray().Sum();
        }

        public static double Range(OneDimArray arr)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            if (arr.Length == 0) throw new InvalidOperationException("Array is empty");
            var data = arr.ToArray();
            return data.Max() - data.Min();
        }

        public static int Count(OneDimArray arr)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            return arr.Length;
        }
    }

    public static class Extensions
    {
        // Удаление гласных (русские + латинские). Возвращает непустую строку (пустая строка вместо null).
        public static string RemoveVowels(this string? s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            const string vowels = "aeiouyаеёиоуыэюяAEIOUYАЕЁИОУЫЭЮЯ";
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
                if (!vowels.Contains(ch)) sb.Append(ch);
            return sb.ToString();
        }

        // Удаление первых пяти элементов массива
        public static OneDimArray RemoveFirstFive(this OneDimArray arr)
        {
            if (arr is null) throw new ArgumentNullException(nameof(arr));
            if (arr.Length <= 5) return new OneDimArray();
            var newLen = arr.Length - 5;
            var data = new double[newLen];
            for (int i = 0; i < newLen; i++) data[i] = arr[i + 5];
            return new OneDimArray(data) { ProductionInfo = arr.ProductionInfo, DeveloperInfo = arr.DeveloperInfo };
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var a = new OneDimArray(1.0, 2.0, 3.5, 4.0, 5.0, 6.0);
            var b = new OneDimArray(10.0, 20.0, 30.0);

            Console.WriteLine("a = " + a);
            Console.WriteLine("b = " + b);

            // -
            var aMinus2 = a - 2.0;
            Console.WriteLine("a - 2 = " + aMinus2);

            // +
            var c = a + b;
            Console.WriteLine("a + b = " + c);

            // >
            Console.WriteLine("a > 3.5 -> " + (a > 3.5));
            Console.WriteLine("a > 99 -> " + (a > 99.0));

            // == !=
            var aCopy = new OneDimArray(1.0, 2.0, 3.5, 4.0, 5.0, 6.0);
            Console.WriteLine("a == aCopy -> " + (a == aCopy));
            Console.WriteLine("a != b -> " + (a != b));

            // StatisticOperation
            Console.WriteLine("Sum(a) = " + StatisticOperation.Sum(a));
            Console.WriteLine("Range(a) = " + StatisticOperation.Range(a));
            Console.WriteLine("Count(a) = " + StatisticOperation.Count(a));

            // Расширения
            string s = "Пример строки: Hello, мир!";
            Console.WriteLine("Original: " + s);
            Console.WriteLine("Without vowels: " + s.RemoveVowels());

            var removedFirstFive = a.RemoveFirstFive();
            Console.WriteLine("a after RemoveFirstFive: " + removedFirstFive);

            // Вложенные объекты
            Console.WriteLine("Production of a: " + a.ProductionInfo);
            Console.WriteLine("Developer of a: " + a.DeveloperInfo);

            // Индексатор
            Console.WriteLine("a[2] = " + a[2]);
            a[2] = 99.9;
            Console.WriteLine("After set a[2] = 99.9, a = " + a);

            // Пример: маленький массив -> RemoveFirstFive -> пустой
            var small = new OneDimArray(1, 2, 3, 4, 5);
            Console.WriteLine("small = " + small);
            Console.WriteLine("small.RemoveFirstFive() = " + small.RemoveFirstFive() + " (Length = " + small.RemoveFirstFive().Length + ")");
        }
    }
}
