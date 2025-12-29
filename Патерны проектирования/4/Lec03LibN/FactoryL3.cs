namespace Lec03LibN
{
    internal class FactoryL3 : IFactory
    {
        private float a;
        private float b;

        public FactoryL3(float a, float b)
        {
            this.a = a;
            this.b = b;
        }

        public IBonus getA(float cost1hour) => new BonusL3A(cost1hour, a, b);

        public IBonus getB(float cost1hour, float x) => new BonusL3B(cost1hour, x, a, b);

        public IBonus getC(float cost1hour, float x, float y) =>
            new BonusL3C(cost1hour, x, y, a, b);
    }

    internal class BonusL3A : IBonus
    {
        public float cost1hour { get; set; }
        private float a;
        private float b;

        public BonusL3A(float cost1hour, float a, float b)
        {
            this.cost1hour = cost1hour;
            this.a = a;
            this.b = b;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * (cost1hour + b);
        }
    }

    internal class BonusL3B : IBonus
    {
        public float cost1hour { get; set; }
        private float x;
        private float a;
        private float b;

        public BonusL3B(float cost1hour, float x, float a, float b)
        {
            this.cost1hour = cost1hour;
            this.x = x;
            this.a = a;
            this.b = b;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * (cost1hour + b) * x;
        }
    }

    internal class BonusL3C : IBonus
    {
        public float cost1hour { get; set; }
        private float x;
        private float y;
        private float a;
        private float b;

        public BonusL3C(float cost1hour, float x, float y, float a, float b)
        {
            this.cost1hour = cost1hour;
            this.x = x;
            this.y = y;
            this.a = a;
            this.b = b;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * (cost1hour + b) * x + y;
        }
    }
}
