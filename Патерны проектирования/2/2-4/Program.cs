using System;

public class C3
{
    private const string PRIVATE_CONST = "C3_PRIV";
    public const string PUBLIC_CONST = "C3_PUB";
    protected const string PROTECTED_CONST = "C3_PROT";

    private int _privateField;
    public int publicField;
    protected int protectedField;

    private string PrivateProperty { get; set; }
    public string PublicProperty { get; set; }
    protected string ProtectedProperty { get; set; }

    public C3()
    {
        _privateField = 1;
        publicField = 2;
        protectedField = 3;

        PrivateProperty = "C3_priv_def";
        PublicProperty = "C3_pub_def";
        ProtectedProperty = "C3_prot_def";
    }

    public C3(int priv, int pub, int prot,
              string privProp, string pubProp, string protProp)
    {
        _privateField = priv;
        publicField = pub;
        protectedField = prot;

        PrivateProperty = privProp;
        PublicProperty = pubProp;
        ProtectedProperty = protProp;
    }

    public C3(C3 other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        _privateField = other._privateField;
        publicField = other.publicField;
        protectedField = other.protectedField;

        PrivateProperty = other.PrivateProperty;
        PublicProperty = other.PublicProperty;
        ProtectedProperty = other.ProtectedProperty;
    }

    private string PrivateMethod()
    {
        return $"C3.PrivateMethod -> {_privateField}, {PrivateProperty}, const={PRIVATE_CONST}";
    }

    protected string ProtectedMethod()
    {
        return $"C3.ProtectedMethod -> {protectedField}, {ProtectedProperty}, const={PROTECTED_CONST}";
    }

    public virtual void PublicMethod()
    {
        Console.WriteLine("C3.PublicMethod:");
        Console.WriteLine($"  PUBLIC_CONST = {PUBLIC_CONST}");
        Console.WriteLine($"  publicField = {publicField}");
        Console.WriteLine($"  PublicProperty = {PublicProperty}");
        Console.WriteLine($"  {PrivateMethod()}");
        Console.WriteLine($"  {ProtectedMethod()}");
    }

    public int GetPrivateField() => _privateField;
}
public class C4 : C3
{
   
    private const string PRIVATE_CONST = "C4_PRIV";
    public const string PUBLIC_CONST = "C4_PUB";
    protected const string PROTECTED_CONST = "C4_PROT";


    private int _privateFieldC4;
    public int publicFieldC4;
    protected int protectedFieldC4;

    private string PrivatePropertyC4 { get; set; }
    public string PublicPropertyC4 { get; set; }
    protected string ProtectedPropertyC4 { get; set; }

    // Конструктор по умолчанию
    public C4() : base()
    {
        _privateFieldC4 = 10;
        publicFieldC4 = 20;
        protectedFieldC4 = 30;

        PrivatePropertyC4 = "C4_priv_def";
        PublicPropertyC4 = "C4_pub_def";
        ProtectedPropertyC4 = "C4_prot_def";
    }

    public C4(int basePriv, int basePub, int baseProt,
              string basePrivProp, string basePubProp, string baseProtProp,
              int c4Priv, int c4Pub, int c4Prot,
              string c4PrivProp, string c4PubProp, string c4ProtProp)
        : base(basePriv, basePub, baseProt, basePrivProp, basePubProp, baseProtProp)
    {
        _privateFieldC4 = c4Priv;
        publicFieldC4 = c4Pub;
        protectedFieldC4 = c4Prot;

        PrivatePropertyC4 = c4PrivProp;
        PublicPropertyC4 = c4PubProp;
        ProtectedPropertyC4 = c4ProtProp;
    }

    // Копирующий конструктор
    public C4(C4 other) : base(other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        _privateFieldC4 = other._privateFieldC4;
        publicFieldC4 = other.publicFieldC4;
        protectedFieldC4 = other.protectedFieldC4;

        PrivatePropertyC4 = other.PrivatePropertyC4;
        PublicPropertyC4 = other.PublicPropertyC4;
        ProtectedPropertyC4 = other.ProtectedPropertyC4;
    }

    // Приватный метод C4
    private string PrivateMethodC4()
    {
        return $"C4.PrivateMethod -> {_privateFieldC4}, {PrivatePropertyC4}, const={PRIVATE_CONST}";
    }

    // Защищённый метод C4 (может вызываться в дальнейшем от наследников)
    protected string ProtectedMethodC4()
    {
        return $"C4.ProtectedMethod -> {protectedFieldC4}, {ProtectedPropertyC4}, const={PROTECTED_CONST}";
    }

    // Переопределённый публичный метод: вызывает базовый метод и демонстрирует доступ к protected полям базового класса
    public override void PublicMethod()
    {
        Console.WriteLine("C4.PublicMethod (override):");

        base.PublicMethod();

        Console.WriteLine($"  C4.PUBLIC_CONST = {PUBLIC_CONST}");
        Console.WriteLine($"  publicFieldC4 = {publicFieldC4}");
        Console.WriteLine($"  PublicPropertyC4 = {PublicPropertyC4}");

        Console.WriteLine($"  {PrivateMethodC4()}");
        Console.WriteLine($"  {ProtectedMethodC4()}");

        // Демонстрация доступа к защищённому методу базового класса
        Console.WriteLine($"  (из C4) вызов базового ProtectedMethod: {ProtectedMethod()}");
    }

    // Публичный метод C4 для доступа к приватному полю C4
    public int GetPrivateFieldC4() => _privateFieldC4;
}
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Создание C3 по умолчанию ===");
        C3 baseDefault = new C3();
        baseDefault.PublicMethod();
        Console.WriteLine($"C3.GetPrivateField() = {baseDefault.GetPrivateField()}");

        Console.WriteLine("\n=== Создание C4 по умолчанию ===");
        C4 derivedDefault = new C4();
        derivedDefault.PublicMethod(); 
        derivedDefault.publicField = 100;         
        derivedDefault.publicFieldC4 = 200;      
        derivedDefault.PublicProperty = "B_public_changed";   
        derivedDefault.PublicPropertyC4 = "C4_public_changed"; 
        Console.WriteLine("\nПосле изменения публичных членов (унаследованных и собственных):");
        derivedDefault.PublicMethod();

        Console.WriteLine("\n=== Создание C4 с параметрами ===");
        var c4Param = new C4(
            basePriv: 7, basePub: 8, baseProt: 9,
            basePrivProp: "b_priv", basePubProp: "b_pub", baseProtProp: "b_prot",
            c4Priv: 70, c4Pub: 80, c4Prot: 90,
            c4PrivProp: "c4_priv", c4PubProp: "c4_pub", c4ProtProp: "c4_prot"
        );
        c4Param.PublicMethod();
        Console.WriteLine($"C4.GetPrivateFieldC4() = {c4Param.GetPrivateFieldC4()}");
        Console.WriteLine($"C4.GetPrivateField() [базовый приват через геттер] = {c4Param.GetPrivateField()}");

        Console.WriteLine("\n=== Копирующий конструктор C4 ===");
        var c4Copy = new C4(c4Param);
        c4Copy.PublicMethod();

        Console.WriteLine("\nГотово.");
    }
}
