namespace Lec03LibN
{
    public static partial class Lec03BLib
    {
        public static IFactory getL1()
        {
            return new FactoryL1();
        }

        public static IFactory getL2(float a)
        {
            return new FactoryL2(a);
        }

        public static IFactory getL3(float a, float b)
        {
            return new FactoryL3(a, b);
        }
    }
}
