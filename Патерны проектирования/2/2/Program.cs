//разобраться со статик 2 создать библиотеку класс один с internal и puclic и :this  написать свой деоегат для event 
using System;
using library;
public class C1
{
    private const string PRIVATE_CONST = "priv_const";
    public const string PUBLIC_CONST = "pub_const";
    protected const string PROTECTED_CONST = "prot_const";

    private int _privateField;
    public int publicField;
    protected int protectedField;

    private string PrivateProperty { get; set; }
    public string PublicProperty { get; set; }
    protected string ProtectedProperty { get; set; }


    public C1()
    {
        _privateField = 1;
        publicField = 2;
        protectedField = 3;

        PrivateProperty = "priv_prop_default";
        PublicProperty = "pub_prop_default";
        ProtectedProperty = "prot_prop_default";
    }

    public C1(int privField, int pubField, int protField,
              string privProp, string pubProp, string protProp)
    {
        _privateField = privField;
        publicField = pubField;
        protectedField = protField;

        PrivateProperty = privProp;
        PublicProperty = pubProp;
        ProtectedProperty = protProp;
    }

   
    public C1(C1 other)
    {
        _privateField = other._privateField;
        publicField = other.publicField;
        protectedField = other.protectedField;

        PrivateProperty = other.PrivateProperty;
        PublicProperty = other.PublicProperty;
        ProtectedProperty = other.ProtectedProperty;
    }
    public C1(int pubField, string pubProp) : this()
    {
        // :this() вызывает конструктор по умолчанию C1()
        // Сначала все поля инициализируются значениями по умолчанию,
        // затем мы переопределяем только публичные поля:
        publicField = pubField;
        PublicProperty = pubProp;
    }
    private string PrivateMethod()
    {
        return $"PrivateMethod: {_privateField}, {PrivateProperty}, const={PRIVATE_CONST}";
    }

    // Защищённый метод (может использоваться в наследниках)
    protected string ProtectedMethod()
    {
        return $"ProtectedMethod: {protectedField}, {ProtectedProperty}, const={PROTECTED_CONST}";
    }

    // Публичный метод — демонстрирует значения и вызывает приватный/защищённый методы
    public void ShowInfo()
    {
        Console.WriteLine("=== ShowInfo (public) ===");
        Console.WriteLine($"PUBLIC_CONST = {PUBLIC_CONST}");
        Console.WriteLine($"publicField = {publicField}");
        Console.WriteLine($"PublicProperty = {PublicProperty}");
        Console.WriteLine(PrivateMethod());         // вызывает приватный метод
        Console.WriteLine(ProtectedMethod());       // вызывает защищённый метод
    }

    // Публичный метод, который меняет приватное поле и свойство
    public void UpdatePrivateMembers(int newPrivateField, string newPrivateProperty)
    {
        _privateField = newPrivateField;
        PrivateProperty = newPrivateProperty;
    }

    public int GetPrivateField() => _privateField;

    // Публичный метод для получения защищённой строки (через возврат ProtectedProperty)
    public string GetProtectedPropertyForDemo() => ProtectedProperty;
}
class Program
{
    static void Main()
    {
        // 1. Конструктор по умолчанию
        var objDefault = new C1();
        Console.WriteLine("Объект создан конструктором по умолчанию:");
        objDefault.ShowInfo();

        // Работа с публичными полями и свойствами
        objDefault.publicField = 20;
        objDefault.PublicProperty = "pub_prop_changed";
        Console.WriteLine("\nПосле изменения публичных членов:");
        objDefault.ShowInfo();

        // 2. Конструктор с параметрами
        var objParam = new C1(
            privField: 10,
            pubField: 11,
            protField: 12,
            privProp: "priv_prop_custom",
            pubProp: "pub_prop_custom",
            protProp: "prot_prop_custom"
        );
        Console.WriteLine("\nОбъект создан конструктором с параметрами:");
        objParam.ShowInfo();

        Console.WriteLine($"\nПриватное поле objParam через метод: {objParam.GetPrivateField()}");

        var objCopy = new C1(objParam);
        Console.WriteLine("\nОбъект создан копирующим конструктором (копия objParam):");
        objCopy.ShowInfo();

        objCopy.UpdatePrivateMembers(99, "priv_prop_from_copy");
        Console.WriteLine("\nПосле изменения приватных членов в копии:");
        objCopy.ShowInfo();

        // Демонстрация доступа к защищённому свойству через публичный метод
        Console.WriteLine($"\nProtectedProperty (для демонстрации) = {objCopy.GetProtectedPropertyForDemo()}");

        // ========== ДЕМОНСТРАЦИЯ КОНСТРУКЦИИ :this ==========
        Console.WriteLine("\n\n========== ДЕМОНСТРАЦИЯ КОНСТРУКЦИИ :this ==========");
        Console.WriteLine("Создаём объект через конструктор C1(int, string), который использует :this()");
        Console.WriteLine("Это означает: сначала выполняется конструктор по умолчанию (инициализация всех полей),");
        Console.WriteLine("затем выполняется тело конструктора (установка публичных полей)");
        var objThis = new C1(pubField: 777, pubProp: "публичное_свойство_через_this");
        objThis.ShowInfo();
        Console.WriteLine("\nВидно, что:");
        Console.WriteLine("  - Приватные поля остались со значениями по умолчанию (1, 'priv_prop_default')");
        Console.WriteLine("  - Публичные поля были установлены нашими значениями (777, 'публичное_свойство_через_this')");
        Console.WriteLine("\n✓ Конструкция :this позволяет переиспользовать логику инициализации");
        Console.WriteLine("  из других конструкторов, избегая дублирования кода.");

        // ========== ДЕМОНСТРАЦИЯ БИБЛИОТЕКИ ==========
        Console.WriteLine("\n\n========== РАБОТА С БИБЛИОТЕКОЙ ==========");
        
        // Использование public класса Class1 из библиотеки
        var libObj1 = new Class1("Инициализация через конструктор");
        Console.WriteLine("Class1 создан с именем через конструктор:");
        libObj1.Print();
        var lib = new Class2;
        // Демонстрация использования this для цепочки вызовов
        var libObj2 = new Class1("Начальное имя")
            .SetName("Имя изменено через SetName")
            .SetName("Финальное имя (цепочка вызовов)");
        Console.WriteLine("\nClass1 с цепочкой вызовов (демонстрация return this):");
        libObj2.Print();

        // Демонстрация работы с this в конструкторе
        var libObj3 = new Class1("Демонстрация this._name в конструкторе");
        libObj3.Print();

        Console.WriteLine("\nБиблиотека успешно подключена и работает!");
        // Примечание: Class2 (internal) недоступен из этого проекта, 
        // так как internal классы видны только внутри той же сборки
    }
}

