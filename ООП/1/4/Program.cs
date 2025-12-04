using System;

class Program
{
    static void Main()
    {
        // a. Задаём кортеж из 5 элементов
        var myTuple = (Id: 42, Name: "Alice", Grade: 'A', City: "Minsk", Score: 123UL);

        // b. Вывод кортежа
        //   целиком
        Console.WriteLine("Полный кортеж: " + myTuple);
        //   выборочно: 1, 3, 4 элементы
        Console.WriteLine($"Выборочно: {myTuple.Item1}, {myTuple.Item3}, {myTuple.Item4}");

        // c. Распаковка в переменные

        // 1) Неявно-типизированная распаковка
        var (id1, name1, grade1, city1, score1) = myTuple;
        Console.WriteLine($"Распаковка #1: {id1}, {name1}, {grade1}, {city1}, {score1}");

        // 2) Явное указание типов
        (int id2, string name2, char grade2, string city2, ulong score2) = myTuple;
        Console.WriteLine($"Распаковка #2: {id2}, {name2}, {grade2}, {city2}, {score2}");

        // 3) Смешанная распаковка (var + явно)
        (var id3, var name3, char grade3, var city3, ulong score3) = myTuple;
        Console.WriteLine($"Распаковка #3: {id3}, {name3}, {grade3}, {city3}, {score3}");

        // 4) Распаковка в уже существующие переменные
        int id4; string name4; char grade4; string city4; ulong score4;
        (id4, name4, grade4, city4, score4) = myTuple;
        Console.WriteLine($"Распаковка #4: {id4}, {name4}, {grade4}, {city4}, {score4}");

        // 5) Использование переменной-«мусорщика» (_)
        //    пропускаем Name и City
        var (id5, _, grade5, _, score5) = myTuple;
        Console.WriteLine($"С дискардом: {id5}, {grade5}, {score5}");

        // d. Сравнение двух кортежей
        var t1 = (1, "A", 'X', "Alpha", 100UL);
        var t2 = (1, "A", 'X', "Alpha", 100UL);
        var t3 = (2, "B", 'Y', "Beta", 200UL);

        Console.WriteLine($"t1 == t2: {t1 == t2}"); // True
        Console.WriteLine($"t1 != t3: {t1 != t3}"); // True
    }
}
