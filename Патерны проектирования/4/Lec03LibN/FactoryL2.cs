namespace Lec03LibN
{
    internal class FactoryL2 : IFactory
    {
        private float a;

        public FactoryL2(float a)
        {
            this.a = a;
        }

        public IBonus getA(float cost1hour) => new BonusL2A(cost1hour, a);

        public IBonus getB(float cost1hour, float x) => new BonusL2B(cost1hour, x, a);

        public IBonus getC(float cost1hour, float x, float y) => new BonusL2C(cost1hour, x, y, a);
    }

    internal class BonusL2A : IBonus
    {
        public float cost1hour { get; set; }
        private float a;

        public BonusL2A(float cost1hour, float a)
        {
            this.cost1hour = cost1hour;
            this.a = a;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * cost1hour;
        }
    }

    internal class BonusL2B : IBonus
    {
        public float cost1hour { get; set; }
        private float x;
        private float a;

        public BonusL2B(float cost1hour, float x, float a)
        {
            this.cost1hour = cost1hour;
            this.x = x;
            this.a = a;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * cost1hour * x;
        }
    }

    internal class BonusL2C : IBonus
    {
        public float cost1hour { get; set; }
        private float x;
        private float y;
        private float a;

        public BonusL2C(float cost1hour, float x, float y, float a)
        {
            this.cost1hour = cost1hour;
            this.x = x;
            this.y = y;
            this.a = a;
        }

        public float calc(float number_hours)
        {
            return (number_hours + a) * cost1hour * x + y;
        }
    }
}
