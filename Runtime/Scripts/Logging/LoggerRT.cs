using UnityEngine;

namespace MG_Utilities
{
    /// <summary>
    /// Global custom logger supporting Rich Text.
    /// </summary>
    public class LoggerRT
    {
        public static bool EnableLogging = true;

        public static void LogInfo(string message, string script)
        {
            if (LoggerConfig.Instance == null)
            {
                Debug.LogError("There is no global logger config added!");
                return;
            }

            if (!EnableLogging) 
                return;
            
            Debug.Log($"[INFO][{script}]: " + message.Color("white")); 
        }

        public static void LogSuccess(string message, string script) 
        {
            if (LoggerConfig.Instance == null)
            {
                Debug.LogError("There is no global logger config added!");
                return;
            }

            if (!EnableLogging)
                return;

            Debug.Log($"[SUCCESS][{script}]: " + message.Color("green")); 
        }

        public static void LogWarning(string message, string script) 
        {
            if (LoggerConfig.Instance == null)
            {
                Debug.LogError("There is no global logger config added!");
                return;
            }

            if (!EnableLogging)
                return;

            Debug.LogWarning($"[WARNING][{script}]: " + message.Color("orange")); 
        }

        public static void LogError(string message, string script) 
        {
            if (LoggerConfig.Instance == null)
            {
                Debug.LogError("There is no global logger config added!");
                return;
            }

            if (!EnableLogging)
                return;

            Debug.LogError($"[ERROR][{script}]: " + message.Color("red")); 
        }
    }
}
