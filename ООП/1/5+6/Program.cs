using System;

class Program
{
    static void Main()
    {
        // 5) Локальная функция: принимает массив int[] и строку, возвращает кортеж
        (int max, int min, int sum, char firstLetter) ProcessData(int[] numbers, string text)
        {
            int maxValue = numbers[0];
            int minValue = numbers[0];
            int total = 0;
            foreach (var n in numbers)
            {
                if (n > maxValue) maxValue = n;
                if (n < minValue) minValue = n;
                total += n;
            }
            char firstChar = text.Length > 0 ? text[0] : '\0';
            return (maxValue, minValue, total, firstChar);
        }

        // Вызов и вывод результатов
        var array = new[] { 5, 3, 9, 1, 7 };
        string input = "Hello";
        var result = ProcessData(array, input);
        Console.WriteLine($"Max: {result.max}, Min: {result.min}, Sum: {result.sum}, First letter: '{result.firstLetter}'");

        // 6) Работа с checked/unchecked

        // a & b) checked-контекст: переполнение вызывает исключение
        void TestCheckedOverflow()
        {
            try
            {
                checked
                {
                    int x = int.MaxValue;
                    x++;  // здесь переполнение
                    Console.WriteLine($"Checked overflow result: {x}");
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Checked context: поймано исключение OverflowException");
            }
        }

        // a & b) unchecked-контекст: результат оборачивается по модулю
        void TestUncheckedOverflow()
        {
            unchecked
            {
                int x = int.MaxValue;
                x++;  // здесь переполнение, но исключения нет
                Console.WriteLine($"Unchecked overflow result: {x}");
            }
        }

        // c) Вызов функций
        TestCheckedOverflow();
        TestUncheckedOverflow();
    }
}
