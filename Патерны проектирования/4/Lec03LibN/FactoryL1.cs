namespace Lec03LibN
{
    internal class FactoryL1 : IFactory
    {
        public IBonus getA(float cost1hour) => new BonusA(cost1hour);

        public IBonus getB(float cost1hour, float x) => new BonusB(cost1hour, x);

        public IBonus getC(float cost1hour, float x, float y) => new BonusC(cost1hour, x, y);
    }

    internal class BonusA : IBonus
    {
        public float cost1hour { get; set; }

        public BonusA(float cost1hour)
        {
            this.cost1hour = cost1hour;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour;
        }
    }

    internal class BonusB : IBonus
    {
        public float cost1hour { get; set; }
        private float x;

        public BonusB(float cost1hour, float x)
        {
            this.cost1hour = cost1hour;
            this.x = x;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour * x;
        }
    }

    internal class BonusC : IBonus
    {
        public float cost1hour { get; set; }
        private float x;
        private float y;

        public BonusC(float cost1hour, float x, float y)
        {
            this.cost1hour = cost1hour;
            this.x = x;
            this.y = y;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour * x + y;
        }
    }
}
