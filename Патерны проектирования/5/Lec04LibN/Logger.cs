namespace Lec04LibN
{
    public class Logger : ILogger
    {
        private string _logFileName;
        private static Logger? _instance = null;
        private Stack<string> _titles = new Stack<string>();

        private Logger()
        {
            this._logFileName =
                $"{AppDomain.CurrentDomain.BaseDirectory}/LOG_{DateTime.Now:yyyyMMdd-HH-mm-ss}.txt";
        }

        public static Logger Create()
        {
            
            _instance ??= new Logger();
            Console.WriteLine($"{DateTime.Now:yyyyMMdd-HH-mm-ss}-INIT");
            Logger logger = _instance!;
            using (StreamWriter st = new StreamWriter(logger._logFileName, true))
            {
                st.WriteLine($"{DateTime.Now:yyyyMMdd-HH-mm-ss}-INIT");
            }
            return logger;
        }

        public void Start(string title = "TITLE")
        {
            this._titles.Push(title);
            Console.WriteLine(
                $"{DateTime.Now:yyyyMMdd-HH-mm-ss}-STARTED {string.Join(":", this._titles.Reverse())}"
            );
            using (StreamWriter st = new StreamWriter(this._logFileName, true))
            {
                st.WriteLine(
                    $"{DateTime.Now:yyyyMMdd-HH-mm-ss}-STARTED {string.Join(":", this._titles.Reverse())}\n"
                );
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(
                $"{DateTime.Now:yyyyMMdd-HH-mm-ss}-INFO {string.Join(":", this._titles.Reverse())} {message}"
            );
            using (StreamWriter st = new StreamWriter(this._logFileName, true))
            {
                st.WriteLine(
                    $"{DateTime.Now:yyyyMMdd-HH-mm-ss}-INFO {string.Join(":", this._titles.Reverse())} {message}\n"
                );
            }
        }

        public void Stop()
        {
            string removedTitle = this._titles.Pop();
            Console.WriteLine($"{DateTime.Now:yyyyMMdd-HH-mm-ss}-STOPED {removedTitle}");
            using (StreamWriter st = new StreamWriter(this._logFileName, true))
            {
                st.WriteLine($"{DateTime.Now:yyyyMMdd-HH-mm-ss}-STOPED {removedTitle}\n");
            }
        }
    }
}
