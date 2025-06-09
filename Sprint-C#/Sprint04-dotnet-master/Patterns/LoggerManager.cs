namespace Sessions_app.Patterns
{
    public sealed class LoggerManager
    {
        private static LoggerManager _instance;
        private static readonly object _lock = new object();
        private string _logFilePath;

        // Construtor privado para evitar instanciação direta
        private LoggerManager()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "application.log");

            // Garantir que o diretório de logs existe
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
        }

        // Método para obter a instância única
        public static LoggerManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LoggerManager();
                    }
                }
            }
            return _instance;
        }

        // Método para registrar logs
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        private void Log(string level, string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_logFilePath))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] - {message}");
                }
            }
            catch (Exception ex)
            {
                // Falha silenciosa, não há muito o que fazer se o log falhar
                Console.WriteLine($"Falha ao gravar log: {ex.Message}");
            }
        }
    }
}
