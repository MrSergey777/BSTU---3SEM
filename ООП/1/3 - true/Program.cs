using System;

class Program
{
    static void Main()
    {
        //a
        int[,] matrix = {
            { 1,  2,  3 },
            { 4,  5,  6 },
            { 7,  8,  9 }
        };

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j],4}");
            }
            Console.WriteLine();
        }
        //b
        string[] words = { "apple", "banana", "cherry", "date" };
        Console.WriteLine("Содержимое массива: " + string.Join(", ", words));
        Console.WriteLine("Длина массива: " + words.Length);

        Console.Write("Введите индекс для замены (0…3): ");
        int idx = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите новое значение: ");
        string? newValue = Console.ReadLine();

        if (idx >= 0 && idx < words.Length && newValue != null)
        {
            words[idx] = newValue;
        }

        Console.WriteLine("Обновлённый массив: " + string.Join(", ", words));
        //c
        double[][] ragged = new double[3][];
        ragged[0] = new double[2];
        ragged[1] = new double[3];
        ragged[2] = new double[4];

        for (int i = 0; i < ragged.Length; i++)
        {
            Console.WriteLine($"Введите {ragged[i].Length} чисел для строки {i}:");
            for (int j = 0; j < ragged[i].Length; j++)
            {
                ragged[i][j] = double.Parse(Console.ReadLine() ?? "0");
            }
        }

        Console.WriteLine("Введённый рваный массив:");
        for (int i = 0; i < ragged.Length; i++)
        {
            Console.WriteLine(string.Join(" ", ragged[i]));
        }
        //d
        var numbers = new[] { 10, 20, 30, 40 };    
        var message = "Пример неявно типизированной строки";

        Console.WriteLine($"Тип numbers: {numbers.GetType().Name}");
        Console.WriteLine($"Тип message: {message.GetType().Name}");
    }
}
