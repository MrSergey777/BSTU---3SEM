namespace Lab5Lib
{
    public class Decorator : IWriter
    {
        protected IWriter? writer;
        protected const char Token = '\uffff';

        public Decorator(IWriter writer)
        {
            this.writer = writer;
        }

        public virtual string? Save(string? message)
        {
            return writer?.Save(message);
        }
    }
}
