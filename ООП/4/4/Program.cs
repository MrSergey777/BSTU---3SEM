#nullable enable
using System;
using System.Text;

// --- Общие интерфейсы и абстрактные базовые классы ---

// Интерфейс управления (show, input, resize и т.д.)
public interface IControl
{
    void Show();
    void Input(string data);
    void Resize(double factor);
}

// Интерфейс для демонстрации одноимённого метода DoClone
public interface ICloneableCustom
{
    bool DoClone();
}

// Абстрактный класс геометрической фигуры
public abstract class GeometricFigure : ICloneableCustom
{
    public string Name { get; protected set; }

    protected GeometricFigure(string name) => Name = name;

    public abstract double Area();
    public abstract double Perimeter();

    public virtual void Describe()
    {
        Console.WriteLine($"Figure {Name}: area={Area():G4}, perimeter={Perimeter():G4}");
    }

    // Абстрактный одноимённый метод DoClone (для демонстрации разной реализации versus интерфейс)
    public abstract bool DoClone();

    public override string ToString() => $"{GetType().Name}: Name={Name}, Area={Area():G4}, Perimeter={Perimeter():G4}";
}

// Абстрактный класс для демонстрации наследования/полиморфизма управления
public abstract class ControlElement : IControl
{
    public string Id { get; protected set; }
    public bool Enabled { get; set; } = true;

    protected ControlElement(string id) => Id = id;

    public virtual void Show() => Console.WriteLine($"{GetType().Name} [{Id}] shown; Enabled={Enabled}");
    public virtual void Input(string data) => Console.WriteLine($"{GetType().Name} [{Id}] input: {data}");
    public virtual void Resize(double factor) => Console.WriteLine($"{GetType().Name} [{Id}] resized by {factor:G2}x");

    public override string ToString() => $"{GetType().Name}: Id={Id}, Enabled={Enabled}";
}

// --- Фигуры ---

// Класс, в котором переопределены все методы от Object
public sealed class FigureWithAllObjectOverrides : GeometricFigure
{
    public double Radius { get; }

    public FigureWithAllObjectOverrides(double radius) : base("FigureWithAllObjectOverrides")
    {
        Radius = radius;
    }

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;

    // Реализация DoClone как требование от абстрактного класса
    public override bool DoClone()
    {
        // демонстрационная логика возврата true
        return true;
    }

    // Переопределяем Equals, GetHashCode, ToString уже сделан в базовом; переопределим все:
    public override bool Equals(object? obj)
    {
        if (obj is not FigureWithAllObjectOverrides other) return false;
        return Radius.Equals(other.Radius);
    }

    public override int GetHashCode() => Radius.GetHashCode() ^ GetType().GetHashCode();

    // Переопределяем ToString (наследуем от GeometricFigure, но предоставим более подробный)
    public override string ToString() =>
        $"{GetType().Name}: Radius={Radius:G4}, Area={Area():G4}, Perimeter={Perimeter():G4}";

    // Destructor (наследник Finalize)
    ~FigureWithAllObjectOverrides()
    {
        // демонстрационная — не пытайтесь полагаться на финализатор для логики
    }
}

// Круг
public class Circle : GeometricFigure
{
    public double Radius { get; private set; }

    public Circle(double radius) : base("Circle")
    {
        Radius = radius;
    }

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;

    public override bool DoClone()
    {
        // реализация абстрактного DoClone в классе Circle (например, клонирование по значению)
        return true;
    }

    public override string ToString() => $"{GetType().Name}: Radius={Radius:G4}, Area={Area():G4}, Perimeter={Perimeter():G4}";
}

// Прямоугольник
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

    public override bool DoClone()
    {
        // другая реализация DoClone
        return false;
    }

    public override string ToString() =>
        $"{GetType().Name}: Width={Width:G4}, Height={Height:G4}, Area={Area():G4}, Perimeter={Perimeter():G4}";
}

// --- Элементы управления ---

// Checkbox
public class Checkbox : ControlElement
{
    public bool Checked { get; private set; }

    public Checkbox(string id, bool isChecked = false) : base(id) => Checked = isChecked;

    public void Toggle() => Checked = !Checked;

    public override void Show() => Console.WriteLine($"{GetType().Name} [{Id}] Checked={Checked}; Enabled={Enabled}");

    public override string ToString() => $"{GetType().Name}: Id={Id}, Checked={Checked}, Enabled={Enabled}";
}

// RadioButton
public class Radiobutton : ControlElement
{
    public string Group { get; private set; }
    public bool Selected { get; private set; }

    public Radiobutton(string id, string group, bool selected = false) : base(id)
    {
        Group = group;
        Selected = selected;
    }

    public void Select() => Selected = true;
    public void Deselect() => Selected = false;

    public override void Show() => Console.WriteLine($"{GetType().Name} [{Id}] Group={Group} Selected={Selected}; Enabled={Enabled}");

    public override string ToString() => $"{GetType().Name}: Id={Id}, Group={Group}, Selected={Selected}, Enabled={Enabled}";
}

// Sealed Button
public sealed class Button : ControlElement
{
    public string Caption { get; private set; }

    public Button(string id, string caption) : base(id) => Caption = caption;

    public void Click() => Console.WriteLine($"Button [{Id}] '{Caption}' clicked");

    public override string ToString() => $"{GetType().Name}: Id={Id}, Caption='{Caption}', Enabled={Enabled}";
}

// --- Класс, который одновременно реализует интерфейс ICloneableCustom и наследует абстрактный GeometricFigure с одноимённым методом DoClone
public class ConflictingCloneClass : GeometricFigure, ICloneableCustom
{
    public string Data { get; private set; }

    public ConflictingCloneClass(string data) : base("ConflictingCloneClass")
    {
        Data = data;
    }

    // Реализация абстрактного метода DoClone (от GeometricFigure)
    public override bool DoClone()
    {
        Console.WriteLine("DoClone from abstract base called (GeometricFigure.DoClone)");
        // логика клонирования - демонстрация
        return true;
    }

    // Реализация интерфейсного DoClone (ICloneableCustom)
    bool ICloneableCustom.DoClone()
    {
        Console.WriteLine("DoClone from interface ICloneableCustom called");
        // иная логика
        return false;
    }

    public override double Area() => 0;
    public override double Perimeter() => 0;

    public override string ToString() => $"{GetType().Name}: Data='{Data}'";
}

// --- Printer с полиморфным методом IAmPrinting ---

public class Printer
{
    // Принимаем наиболее общий тип — абстрактный класс GeometricFigure (вариант в задании)
    public void IAmPrinting(GeometricFigure someObj)
    {
        if (someObj is null) { Console.WriteLine("Null reference passed"); return; }

        // Определение типа через is/as
        Console.WriteLine($"Printer detected runtime type: {someObj.GetType().Name}");
        Console.WriteLine(someObj.ToString());
    }

    // Перегрузка для элементов управления
    public void IAmPrinting(ControlElement control)
    {
        if (control is null) { Console.WriteLine("Null control passed"); return; }
        Console.WriteLine($"Printer detected runtime type: {control.GetType().Name}");
        Console.WriteLine(control.ToString());
    }
}

// --- Демонстрационная программа ---

public static class Demo
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Создаём массив фигур (разнотипные объекты)
        GeometricFigure[] figures = new GeometricFigure[]
        {
            new Circle(3.0),
            new Rectangle(4.0, 5.0),
            new FigureWithAllObjectOverrides(2.5),
            new ConflictingCloneClass("payload")
        };

        // Создаём массив контролов (разнотипные объекты)
        ControlElement[] controls = new ControlElement[]
        {
            new Checkbox("cb1", true),
            new Radiobutton("rb1", "g1", true),
            new Button("btn1", "OK")
        };

        // Создаём Printer
        var printer = new Printer();

        Console.WriteLine("=== Printing Figures via Printer (use of is/as) ===");
        foreach (var f in figures)
        {
            // Пример использования is/as для идентификации
            if (f is Circle c)
            {
                Console.WriteLine("Identified as Circle via is: radius = " + c.Radius);
            }
            else if (f is Rectangle r)
            {
                Console.WriteLine("Identified as Rectangle via is: size = " + r.Width + "x" + r.Height);
            }
            else if (f is ConflictingCloneClass conflict)
            {
                Console.WriteLine("Identified as ConflictingCloneClass via is: data = " + conflict.Data);
            }
            else if (f is FigureWithAllObjectOverrides fo)
            {
                Console.WriteLine("Identified as FigureWithAllObjectOverrides via is: radius = " + fo.Radius);
            }

            // Вызов Printer
            printer.IAmPrinting(f);
            Console.WriteLine();
        }

        Console.WriteLine("=== Printing Controls via Printer (polymorphic overload) ===");
        foreach (var ctl in controls)
        {
            // Используем as для попытки приведения
            var btn = ctl as Button;
            if (btn != null)
            {
                Console.WriteLine("This control is a Button via as: caption = " + btn.Caption);
                btn.Click();
            }

            printer.IAmPrinting(ctl);
            Console.WriteLine();
        }

        Console.WriteLine("=== Демонстрация одноимённых DoClone ===");
        var conflictObj = new ConflictingCloneClass("example");
        // Вызов метода DoClone как метода абстрактного класса (вызов реализованного override)
        bool fromAbstract = conflictObj.DoClone();
        Console.WriteLine("DoClone() via abstract method returned: " + fromAbstract);

        // Вызов метода DoClone интерфейсной реализации (через явное приведение к ICloneableCustom)
        var asInterface = (ICloneableCustom)conflictObj;
        bool fromInterface = asInterface.DoClone();
        Console.WriteLine("DoClone() via interface returned: " + fromInterface);

        Console.WriteLine("=== Завершение демонстрации ===");
    }
}
