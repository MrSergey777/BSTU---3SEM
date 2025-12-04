using System;

class Airline
{
    // Константа — максимальное число дней в неделе
    private const int MaxDaysInWeek = 7;

    // Поле только для чтения — уникальный ID рейса
    private readonly Guid ID;

    // Статическое поле — счётчик созданных объектов
    private static int instancesCount;

    // Приватные поля класса
    private string destination;
    private int flightNumber;
    private string planeType;
    private TimeSpan departureTime;
    private DayOfWeek[] days;

    // Статический конструктор
    static Airline()
    {
        instancesCount = 0;
    }

    // Публичный конструктор без параметров
    public Airline()
        : this("Unknown", 0, "Unknown", TimeSpan.Zero, new DayOfWeek[0])
    {
    }

    // Публичный конструктор со всеми параметрами
    public Airline(string destination, int flightNumber, string planeType,
                   TimeSpan departureTime, params DayOfWeek[] days)
    {
        ID = Guid.NewGuid();
        this.destination = destination;
        this.flightNumber = flightNumber;
        this.planeType = planeType;
        this.departureTime = departureTime;
        this.days = days;
        instancesCount++;
    }

    // Публичный конструктор с двумя параметрами (дефолтные для остальных)
    public Airline(string destination, int flightNumber)
        : this(destination, flightNumber, "Boeing-737", new TimeSpan(12, 0, 0), DayOfWeek.Monday)
    {
    }

    // Приватный конструктор
    private Airline(string destination)
        : this(destination, -1, "TestPlane", TimeSpan.Zero, DayOfWeek.Sunday)
    {
    }

    // Фабричный метод для вызова приватного конструктора
    public static Airline CreateForTest(string destination)
    {
        return new Airline(destination);
    }

    // Свойства (без проверок)
    public string Destination
    {
        get => destination;
        set => destination = value;
    }

    public int FlightNumber
    {
        get => flightNumber;
        private set => flightNumber = value;
    }

    public string PlaneType
    {
        get => planeType;
        set => planeType = value;
    }

    public TimeSpan DepartureTime
    {
        get => departureTime;
        set => departureTime = value;
    }

    public DayOfWeek[] Days
    {
        get => days;
        set => days = value;
    }

    // Метод с ref и out параметрами
    public void TryReschedule(ref TimeSpan newTime, out bool success)
    {
        departureTime = newTime;
        success = true;
    }

    // Статический метод вывода информации о классе
    public static void ShowClassInfo()
    {
        Console.WriteLine($"Airline: создано объектов = {instancesCount}, MaxDaysInWeek = {MaxDaysInWeek}");
    }

    // Переопределяем Equals по ID
    public override bool Equals(object obj)
    {
        if (obj is Airline other)
            return this.ID == other.ID;
        return false;
    }

    // Переопределяем GetHashCode по ID
    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    // Переопределяем ToString для показа информации
    public override string ToString()
    {
        var daysList = days == null
            ? ""
            : string.Join(",", days);
        return $"[{ID}] {destination} #{flightNumber}, {planeType}, {departureTime}, Days: {daysList}";
    }
}

class Program
{
    static void Main()
    {
        // Создаём разные рейсы
        var a1 = new Airline();
        var a2 = new Airline("Minsk", 101, "Embraer-190", new TimeSpan(9, 30, 0),
                             DayOfWeek.Monday, DayOfWeek.Wednesday);
        var a3 = Airline.CreateForTest("TestVille");
        var a4 = new Airline("Moscow", 202);
        var a5 = new Airline("Minsk", 303, "Airbus-A320", new TimeSpan(15, 45, 0),
                             DayOfWeek.Friday);

        // Собираем в массив
        Airline[] flights = { a1, a2, a3, a4, a5 };

        // a) список рейсов для заданного пункта назначения
        string queryDest = "Minsk";
        Console.WriteLine($"Рейсы в {queryDest}:");
        foreach (var f in flights)
            if (f.Destination == queryDest)
                Console.WriteLine(f);

        // b) список рейсов для заданного дня недели
        DayOfWeek queryDay = DayOfWeek.Friday;
        Console.WriteLine($"\nРейсы по {queryDay}:");
        foreach (var f in flights)
            if (f.Days != null && Array.Exists(f.Days, d => d == queryDay))
                Console.WriteLine(f);

        // Выводим статистику класса
        Airline.ShowClassInfo();

        // Анонимный тип по примеру Airline
        var anon = new
        {
            Destination = "Paris",
            FlightNumber = 404,
            PlaneType = "Boeing-777",
            DepartureTime = new TimeSpan(20, 0, 0),
            Days = new[] { DayOfWeek.Saturday }
        };
        Console.WriteLine($"\nАнонимный рейс: {anon.Destination} #{anon.FlightNumber} at {anon.DepartureTime}");
    }
}
