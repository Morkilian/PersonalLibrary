namespace Morkilian
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Morkilian.Helper;

    public static class Logger
    {
        #region LOG
        public static bool DEBUG_LOG = true;
        public static bool DEBUG_LOG_WARNING = true;
        public static bool DEBUG_LOG_ERROR = true;
        
        private static bool CheckEnumFlag(eLoggerTypes category)
        {
            if (MorkilianSettings.HasInstance == false) return true;
            return MorkilianSettings.Instance.DebugMask.ContainsFlag(category);
        }

        private static string LogMessageWithLogger(string message, eLoggerTypes category = eLoggerTypes.Misc, Component logger = null)
        {
            if (logger != null)
                return $"[{category}]-[{logger.name}]-[{logger.GetInstanceID()}] - {message}";
            return $"[{category}] - {message}";
        }

        public static void Log(string message, eLoggerTypes category = eLoggerTypes.Misc, Component logger = null, bool additionalCondition = true)
        {
            if (DEBUG_LOG && additionalCondition && CheckEnumFlag(category))
                Debug.Log(LogMessageWithLogger(message, category, logger), logger);
        }
        public static void LogWarning(string message, eLoggerTypes category = eLoggerTypes.Misc, Component logger = null, bool additionalCondition = true)
        {
            if (DEBUG_LOG_WARNING && additionalCondition && CheckEnumFlag(category))
                Debug.LogWarning(LogMessageWithLogger(message, category, logger), logger);
        }

        public static void LogError(string message, eLoggerTypes category = eLoggerTypes.Misc, Component logger = null, bool additionalCondition = true)
        {
            if (DEBUG_LOG_ERROR && additionalCondition && CheckEnumFlag(category))
                Debug.LogError(LogMessageWithLogger(message, category, logger), logger);
        }
        #endregion
    }

}