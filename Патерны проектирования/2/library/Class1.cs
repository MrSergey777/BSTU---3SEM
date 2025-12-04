namespace library
{
    public class Class1
    {
        private string _name;
        public Class1(string name)
        {
            this._name = name;
        }
        public Class1 SetName(string name)
        {
            this._name = name;
            return this;
        }

        // Сделали метод виртуальным чтобы его можно было переопределять
        public virtual void Print()
        {
            Console.WriteLine(this._name);
        }
    }
    public class Class1Derived : Class1
    {
        private readonly string _prefix;
        public Class1Derived(string name, string prefix) : base(name)
        {
            _prefix = prefix;
        }

        // override заменяет реализацию базового виртуального метода
        public override void Print()
        {
            // можно вызвать реализацию базового класса через base
            base.Print();
            Console.WriteLine($"{_prefix}: дополнительный вывод в производном классе");
        }
    }

    internal class Class2
    {
        private int _value;

        public Class2(int value)
        {
            this._value = value;
        }

        // Можно также сделать Increment виртуальным, если нужно переопределять поведение
        public virtual void Increment()
        {
            this._value++;
        }
        public int GetValue() => this._value;
    }
}
