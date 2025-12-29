namespace Lab5Lib
{
    public class FileWriter : IWriter
    {
        private readonly string path;

        public FileWriter(string path = "output.txt")
        {
            this.path = path;
        }

        public string? Save(string? message)
        {
            using (var st = new StreamWriter(this.path))
            {
                // st.Write($"{DateTime.Now:HH:mm\ndd.MM.yyyy}\n");
                st.Write(message);
                // st.Write("\n");
            }
            return message;
        }
    }
}
