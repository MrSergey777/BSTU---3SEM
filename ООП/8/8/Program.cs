using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using s;
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
}

 class Program
    {
        static void Main()
        {
            Q q = new Q();
            q.aa += (s) => { Console.WriteLine(s.ToCharArray()); return s.ToCharArray(); };
            q.EE("aaaa");
        
    }
}