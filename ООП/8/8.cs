using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab8
{
    // Делегаты для событий
    public delegate void MoveHandler(int offsetX, int offsetY);
    public delegate void CompressHandler(double compressionRatio);

    // Базовый класс для объектов, которые могут реагировать на события
    public abstract class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Scale { get; set; } = 1.0;

        public abstract void Display();
    }

    // Класс Пользователь с событиями
    public class User
    {
        // События
        public event MoveHandler? OnMove;
        public event CompressHandler? OnCompress;

        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }

        // Метод для вызова события перемещения
        public void Move(int offsetX, int offsetY)
        {
            Console.WriteLine($"Пользователь {Name} вызывает событие перемещения: ({offsetX}, {offsetY})");
            OnMove?.Invoke(offsetX, offsetY);
        }

        // Метод для вызова события сжатия
        public void Compress(double compressionRatio)
        {
            Console.WriteLine($"Пользователь {Name} вызывает событие сжатия: {compressionRatio}");
            OnCompress?.Invoke(compressionRatio);
        }
    }

    // Различные типы объектов, реагирующих на события
    public class Rectangle : GameObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public Rectangle(string name, int x, int y, int width, int height)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override void Display()
        {
            Console.WriteLine($"Прямоугольник '{Name}': Позиция ({X}, {Y}), Размер {Width * Scale:F2}x{Height * Scale:F2}, Масштаб: {Scale:F2}");
        }
    }

    public class Circle : GameObject
    {
        public int Radius { get; set; }
        public string Name { get; set; }

        public Circle(string name, int x, int y, int radius)
        {
            Name = name;
            X = x;
            Y = y;
            Radius = radius;
        }

        public override void Display()
        {
            Console.WriteLine($"Круг '{Name}': Позиция ({X}, {Y}), Радиус {Radius * Scale:F2}, Масштаб: {Scale:F2}");
        }
    }

    public class Triangle : GameObject
    {
        public int SideLength { get; set; }
        public string Name { get; set; }

        public Triangle(string name, int x, int y, int sideLength)
        {
            Name = name;
            X = x;
            Y = y;
            SideLength = sideLength;
        }

        public override void Display()
        {
            Console.WriteLine($"Треугольник '{Name}': Позиция ({X}, {Y}), Сторона {SideLength * Scale:F2}, Масштаб: {Scale:F2}");
        }
    }

    // Класс для обработки строк
    public class StringProcessor
    {
        // Методы обработки строки
        public static string RemovePunctuation(string str)
        {
            return new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        public static string AddSymbols(string str)
        {
            return $"***{str}***";
        }

        public static string ToUpperCase(string str)
        {
            return str.ToUpper();
        }

        public static string RemoveExtraSpaces(string str)
        {
            return Regex.Replace(str.Trim(), @"\s+", " ");
        }

        public static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Часть 1: Работа с событиями и делегатами ===\n");

            // Создаем пользователя
            User user = new User("Иван");

            // Создаем объекты разных типов
            Rectangle rect1 = new Rectangle("Прямоугольник 1", 10, 20, 100, 50);
            Rectangle rect2 = new Rectangle("Прямоугольник 2", 30, 40, 80, 60);
            Circle circle1 = new Circle("Круг 1", 50, 60, 25);
            Circle circle2 = new Circle("Круг 2", 70, 80, 30);
            Triangle triangle1 = new Triangle("Треугольник 1", 90, 100, 40);

            // Подписываем объекты на события
            // rect1 подписан на оба события
            user.OnMove += (x, y) =>
            {
                rect1.X += x;
                rect1.Y += y;
                Console.WriteLine($"  -> {rect1.Name} перемещен");
            };
            user.OnCompress += (ratio) =>
            {
                rect1.Scale *= ratio;
                Console.WriteLine($"  -> {rect1.Name} сжат");
            };

            // rect2 подписан только на перемещение
            user.OnMove += (x, y) =>
            {
                rect2.X += x;
                rect2.Y += y;
                Console.WriteLine($"  -> {rect2.Name} перемещен");
            };

            // circle1 подписан на оба события
            user.OnMove += (x, y) =>
            {
                circle1.X += x;
                circle1.Y += y;
                Console.WriteLine($"  -> {circle1.Name} перемещен");
            };
            user.OnCompress += (ratio) =>
            {
                circle1.Scale *= ratio;
                Console.WriteLine($"  -> {circle1.Name} сжат");
            };

            // circle2 подписан только на сжатие
            user.OnCompress += (ratio) =>
            {
                circle2.Scale *= ratio;
                Console.WriteLine($"  -> {circle2.Name} сжат");
            };

            // triangle1 не подписан на события

            Console.WriteLine("Начальное состояние объектов:");
            rect1.Display();
            rect2.Display();
            circle1.Display();
            circle2.Display();
            triangle1.Display();

            Console.WriteLine("\n--- Вызов события перемещения ---");
            user.Move(5, 10);

            Console.WriteLine("\n--- Вызов события сжатия ---");
            user.Compress(0.8);

            Console.WriteLine("\n--- Вызов события перемещения еще раз ---");
            user.Move(-2, 5);

            Console.WriteLine("\nСостояние объектов после событий:");
            rect1.Display();
            rect2.Display();
            circle1.Display();
            circle2.Display();
            triangle1.Display();

            Console.WriteLine("\n\n=== Часть 2: Обработка строк с использованием стандартных делегатов ===\n");

            // Исходная строка
            string originalString = "  Привет, мир! Как дела?  ";

            Console.WriteLine($"Исходная строка: \"{originalString}\"");

            // Используем Func для последовательной обработки
            Func<string, string> processingChain = null;

            // Создаем цепочку обработки
            processingChain += StringProcessor.RemoveExtraSpaces;
            processingChain += StringProcessor.ToUpperCase;
            processingChain += StringProcessor.RemovePunctuation;
            processingChain += StringProcessor.AddSymbols;

            // Применяем цепочку обработки
            string result = originalString;
            foreach (Func<string, string> handler in processingChain.GetInvocationList().Cast<Func<string, string>>())
            {
                result = handler(result);
                Console.WriteLine($"После {handler.Method.Name}: \"{result}\"");
            }

            Console.WriteLine($"\nФинальный результат: \"{result}\"");

            // Используем Action для операций без возврата значения
            Console.WriteLine("\n--- Использование Action для вывода информации ---");
            Action<string> printAction = (str) => Console.WriteLine($"Длина строки: {str.Length}");
            printAction(result);

            // Используем Predicate для проверки условий
            Console.WriteLine("\n--- Использование Predicate для проверки условий ---");
            Predicate<string> hasDigits = (str) => str.Any(char.IsDigit);
            Predicate<string> isLong = (str) => str.Length > 10;

            Console.WriteLine($"Строка содержит цифры: {hasDigits(result)}");
            Console.WriteLine($"Строка длиннее 10 символов: {isLong(result)}");

            // Демонстрация всех методов обработки строк по отдельности
            Console.WriteLine("\n--- Все методы обработки строк по отдельности ---");
            string testString = "  Тестовая строка, с пробелами!  ";
            Console.WriteLine($"Тестовая строка: \"{testString}\"");
            Console.WriteLine($"Удаление знаков препинания: \"{StringProcessor.RemovePunctuation(testString)}\"");
            Console.WriteLine($"Добавление символов: \"{StringProcessor.AddSymbols(testString)}\"");
            Console.WriteLine($"Заглавные буквы: \"{StringProcessor.ToUpperCase(testString)}\"");
            Console.WriteLine($"Удаление лишних пробелов: \"{StringProcessor.RemoveExtraSpaces(testString)}\"");
            Console.WriteLine($"Разворот строки: \"{StringProcessor.Reverse(testString)}\"");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}

