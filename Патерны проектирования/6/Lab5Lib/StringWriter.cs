namespace Lab5Lib
{
    public class StrWriter : IWriter
    {
        private string? message;

        public StrWriter() { }

        public string? Save(string? message)
        {
            this.message = message;
            return message;
        }
    }
}
