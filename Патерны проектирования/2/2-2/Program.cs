using System;
using System.Collections.Generic;
public delegate void WorkCompletedHandler(object sender, EventArgs e);

public interface I1
{
    string Name { get; set; }
    void DoWork(int times);
    event WorkCompletedHandler WorkCompleted;
    string this[int index] { get; set; }
}

public class C2 : I1
{
    // Константы
    private const string PRIVATE_CONST = "priv_const";
    public const string PUBLIC_CONST = "pub_const";
    protected const string PROTECTED_CONST = "prot_const";

    // Поля
    private int _privateField;
    public int publicField;
    protected int protectedField;

    private string PrivateProperty { get; set; }
    public string PublicProperty { get; set; }
    protected string ProtectedProperty { get; set; }

    public string Name { get; set; }

    // Событие на базе собственного делегата
    public event WorkCompletedHandler WorkCompleted;

    // Внутреннее хранилище для индексатора и демонстрации DoWork
    private List<string> _items = new List<string>();

    // Индексатор интерфейса I1
    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count) return null;
            return _items[index];
        }
        set
        {
            if (index < 0) return;
            if (index < _items.Count) _items[index] = value;
            else
            {
                while (_items.Count < index) _items.Add(null);
                _items.Add(value);
            }
        }
    }

    // Конструктор по умолчанию
    public C2()
    {
        _privateField = 1;
        publicField = 2;
        protectedField = 3;

        PrivateProperty = "priv_prop_default";
        PublicProperty = "pub_prop_default";
        ProtectedProperty = "prot_prop_default";

        Name = "DefaultName";
    }

    // Конструктор с параметрами
    public C2(int privField, int pubField, int protField,
              string privProp, string pubProp, string protProp,
              string name = "Named")
    {
        _privateField = privField;
        publicField = pubField;
        protectedField = protField;

        PrivateProperty = privProp;
        PublicProperty = pubProp;
        ProtectedProperty = protProp;

        Name = name;
    }

    // Копирующий конструктор
    public C2(C2 other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        _privateField = other._privateField;
        publicField = other.publicField;
        protectedField = other.protectedField;

        PrivateProperty = other.PrivateProperty;
        PublicProperty = other.PublicProperty;
        ProtectedProperty = other.ProtectedProperty;

        Name = other.Name;
        _items = new List<string>(other._items);
    }

    private string PrivateMethod()
    {
        return $"PrivateMethod: {_privateField}, {PrivateProperty}, const={PRIVATE_CONST}";
    }

    protected string ProtectedMethod()
    {
        return $"ProtectedMethod: {protectedField}, {ProtectedProperty}, const={PROTECTED_CONST}";
    }

    public void ShowInfo()
    {
        Console.WriteLine("=== C2 ShowInfo ===");
        Console.WriteLine($"PUBLIC_CONST = {PUBLIC_CONST}");
        Console.WriteLine($"publicField = {publicField}");
        Console.WriteLine($"PublicProperty = {PublicProperty}");
        Console.WriteLine($"Name = {Name}");
        Console.WriteLine(PrivateMethod());
        Console.WriteLine(ProtectedMethod());
    }

    // Публичный метод для изменения приватных членов
    public void UpdatePrivateMembers(int newPrivateField, string newPrivateProperty)
    {
        _privateField = newPrivateField;
        PrivateProperty = newPrivateProperty;
    }

    public int GetPrivateField() => _privateField;

    // Реализация метода интерфейса I1
    public void DoWork(int times)
    {
        for (int i = 0; i < times; i++)
        {
            string item = $"{Name ?? "Worker"}-item-{i}";
            _items.Add(item);
            Console.WriteLine($"Создан элемент {_items.Count - 1}: {item}");
        }

        // Вызов события по завершении
        if (WorkCompleted != null) WorkCompleted(this, EventArgs.Empty);
    }

    // Публичный метод для получения защищённого свойства (демо доступа)
    public string GetProtectedPropertyForDemo() => ProtectedProperty;
}

class Program
{
    static void Main()
    {
        // 1) Конструктор по умолчанию
        var objDefault = new C2();
        Console.WriteLine("Объект C2 создан конструктором по умолчанию:");
        objDefault.ShowInfo();

        // Работа с публичными полями и свойствами
        objDefault.publicField = 20;
        objDefault.PublicProperty = "pub_prop_changed";
        objDefault.Name = "ProcessorA";
        Console.WriteLine("\nПосле изменения публичных членов:");
        objDefault.ShowInfo();

        // Подписка на событие WorkCompleted (используется собственный делегат)
        objDefault.WorkCompleted += (s, e) => Console.WriteLine("Событие WorkCompleted получено (objDefault).");

        // Работа индексатора и DoWork
        objDefault.DoWork(2); // создаст элементы 0 и 1
        Console.WriteLine($"Элемент 0 через индексатор: {objDefault[0]}");
        objDefault[1] = "Custom-item-1";
        Console.WriteLine($"Элемент 1 после записи: {objDefault[1]}");
        objDefault[4] = "Item-4"; // добавление за пределами текущего размера
        Console.WriteLine($"Элемент 4: {objDefault[4]}");

        Console.WriteLine("\nТекущие элементы в objDefault:");
        for (int i = 0; ; i++)
        {
            var v = objDefault[i];
            if (v == null) break;7
            Console.WriteLine($"[{i}] = {v}");
        }

        // 2) Конструктор с параметрами
        var objParam = new C2(10, 11, 12, "priv_prop_custom", "pub_prop_custom", "prot_prop_custom", "ParamName");
        objParam.WorkCompleted += (s, e) => Console.WriteLine("Событие WorkCompleted получено (objParam).");
        Console.WriteLine("\nОбъект C2 создан конструктором с параметрами:");
        objParam.ShowInfo();
        objParam.DoWork(1);

        // Доступ к приватному полю через публичный геттер
        Console.WriteLine($"\nПриватное поле objParam через метод: {objParam.GetPrivateField()}");

        // 3) Копирующий конструктор
        var objCopy = new C2(objParam);
        Console.WriteLine("\nОбъект C2 создан копирующим конструктором (копия objParam):");
        objCopy.ShowInfo();

        // Изменим приватные члены через публичный метод
        objCopy.UpdatePrivateMembers(99, "priv_prop_from_copy");
        Console.WriteLine("\nПосле изменения приватных членов в копии:");
        objCopy.ShowInfo();

        Console.WriteLine($"\nProtectedProperty (для демонстрации) = {objCopy.GetProtectedPropertyForDemo()}");
    }
}
