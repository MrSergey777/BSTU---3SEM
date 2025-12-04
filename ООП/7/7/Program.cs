#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// 1. Обобщенный интерфейс с операциями добавить, удалить, просмотреть
public interface ICollectionOperations<T>
{
    void Add(T item);
    bool Remove(T item);
    void Display();
    IEnumerable<T> Find(Predicate<T> predicate);
}

// 2. Обобщенный класс CollectionType<T> с ограничением where T : class
public class CollectionType<T> : ICollectionOperations<T> where T : class
{
    private List<T> _collection;

    public CollectionType()
    {
        _collection = new List<T>();
    }

    public CollectionType(IEnumerable<T> items)
    {
        _collection = new List<T>(items);
    }

    // Реализация интерфейса ICollectionOperations<T>
    public void Add(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _collection.Add(item);
            Console.WriteLine($"Элемент добавлен: {item}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция добавления завершена");
        }
    }

    public bool Remove(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            bool removed = _collection.Remove(item);
            Console.WriteLine(removed ? $"Элемент удален: {item}" : "Элемент не найден");
            return removed;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция удаления завершена");
        }
    }

    public void Display()
    {
        try
        {
            if (_collection.Count == 0)
            {
                Console.WriteLine("Коллекция пуста");
                return;
            }

            Console.WriteLine($"Коллекция ({typeof(T).Name}):");
            foreach (var item in _collection)
            {
                Console.WriteLine($"  {item}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция отображения завершена");
        }
    }

    public IEnumerable<T> Find(Predicate<T> predicate)
    {
        try
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var results = _collection.FindAll(predicate);
            Console.WriteLine($"Найдено элементов: {results.Count}");
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция поиска завершена");
        }
    }

    // Методы для работы с файлом
    public void SaveToFile(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (var item in _collection)
                {
                    writer.WriteLine(item?.ToString() ?? "null");
                }
            }
            Console.WriteLine($"Коллекция сохранена в файл: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция сохранения в файл завершена");
        }
    }

    public void LoadFromFile(string filename)
    {
        try
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"Файл {filename} не найден");

            var loadedItems = new List<T>();
            string[] lines = File.ReadAllLines(filename, Encoding.UTF8);

            foreach (string line in lines)
            {
                // Для простоты считаем, что T имеет конструктор с string параметром
                // В реальном приложении нужно было бы реализовать парсинг
                if (!string.IsNullOrEmpty(line) && line != "null")
                {
                    // Здесь должна быть более сложная логика десериализации
                    // Для демонстрации просто выводим строки
                    Console.WriteLine($"Прочитано: {line}");
                }
            }

            Console.WriteLine($"Загружено {lines.Length} элементов из файла: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция загрузки из файла завершена");
        }
    }

    // Дополнительные полезные методы
    public int Count => _collection.Count;

    public T this[int index]
    {
        get
        {
            try
            {
                return _collection[index];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при доступе по индексу: {ex.Message}");
                throw;
            }
        }
        set
        {
            try
            {
                _collection[index] = value ?? throw new ArgumentNullException(nameof(value));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при установке по индексу: {ex.Message}");
                throw;
            }
        }
    }

    public void Clear()
    {
        try
        {
            _collection.Clear();
            Console.WriteLine("Коллекция очищена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при очистке: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция очистки завершена");
        }
    }
}

// Классы из лабораторной №4 для использования в качестве параметра обобщения
public abstract class GeometricFigure
{
    public string Name { get; protected set; }

    protected GeometricFigure(string name) => Name = name;

    public abstract double Area();
    public abstract double Perimeter();

    public override string ToString() =>
        $"{GetType().Name}: Name={Name}, Area={Area():G4}, Perimeter={Perimeter():G4}";
}

public class Circle : GeometricFigure
{
    public double Radius { get; private set; }

    public Circle(double radius) : base("Circle")
    {
        Radius = radius;
    }

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : GeometricFigure
{
    public double Width { get; private set; }
    public double Height { get; private set; }

    public Rectangle(double width, double height) : base("Rectangle")
    {
        Width = width;
        Height = height;
    }

    public override double Area() => Width * Height;
    public override double Perimeter() => 2 * (Width + Height);
}

// Демонстрационная программа
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // 3. Тестирование со стандартными типами данных
        Console.WriteLine("=== Тестирование с string ===");
        TestWithStrings();

        Console.WriteLine("\n=== Тестирование с GeometricFigure ===");
        TestWithGeometricFigures();

        Console.WriteLine("\n=== Тестирование с int (через object) ===");
        TestWithIntegers();
    }

    private static void TestWithStrings()
    {
        try
        {
            var stringCollection = new CollectionType<string>();

            // Добавление элементов
            stringCollection.Add("Hello");
            stringCollection.Add("World");
            stringCollection.Add("Test");

            // Отображение
            stringCollection.Display();

            // Поиск по предикату
            var longStrings = stringCollection.Find(s => s.Length > 4);
            Console.WriteLine("Строки длиннее 4 символов:");
            foreach (var str in longStrings)
            {
                Console.WriteLine($"  {str}");
            }

            // Удаление
            stringCollection.Remove("Test");
            stringCollection.Display();

            // Работа с файлом
            stringCollection.SaveToFile("strings.txt");
            stringCollection.LoadFromFile("strings.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в тесте строк: {ex.Message}");
        }
    }

    private static void TestWithGeometricFigures()
    {
        try
        {
            var figureCollection = new CollectionType<GeometricFigure>();

            // Добавление фигур
            figureCollection.Add(new Circle(5.0));
            figureCollection.Add(new Rectangle(4.0, 6.0));
            figureCollection.Add(new Circle(2.5));

            // Отображение
            figureCollection.Display();

            // Поиск по предикату
            var largeFigures = figureCollection.Find(f => f.Area() > 20);
            Console.WriteLine("Фигуры с площадью > 20:");
            foreach (var figure in largeFigures)
            {
                Console.WriteLine($"  {figure}");
            }

            // Работа с файлом
            figureCollection.SaveToFile("figures.txt");
            figureCollection.LoadFromFile("figures.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в тесте фигур: {ex.Message}");
        }
    }

    private static void TestWithIntegers()
    {
        try
        {
            // Для значимых типов используем object (boxing)
            var intCollection = new CollectionType<object>();

            intCollection.Add(10);
            intCollection.Add(20);
            intCollection.Add(30);

            intCollection.Display();

            // Поиск четных чисел
            var evenNumbers = intCollection.Find(obj =>
                obj is int i && i % 2 == 0);

            Console.WriteLine("Четные числа:");
            foreach (var num in evenNumbers)
            {
                Console.WriteLine($"  {num}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в тесте целых чисел: {ex.Message}");
        }
    }
}