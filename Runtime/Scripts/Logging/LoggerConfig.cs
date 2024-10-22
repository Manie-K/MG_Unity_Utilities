namespace MG_Utilities
{
    public class LoggerConfig : Singleton<LoggerConfig>
    {
        public bool LoggingEnabled => LoggerRT.EnableLogging;
        public void EnableLogging() => LoggerRT.EnableLogging = true;
        public void DisableLogging() => LoggerRT.EnableLogging = false;
    }
}