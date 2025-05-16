﻿using BepInEx.Logging;


namespace GorillaCustomSkin.Tools
{
    public class Logging
    {
        public static void Info(object message) => LogMessage(LogLevel.Info, message);

        public static void Warning(object message) => LogMessage(LogLevel.Warning, message);

        public static void Error(object message) => LogMessage(LogLevel.Error, message);

        public static void Fatal(object message) => LogMessage(LogLevel.Fatal, message);

        public static void LogMessage(LogLevel level, object message)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif
            if (!Constants.DebugLogExclusive || debug)
            {
                Plugin.MainLogSource?.Log(level, message);
            }
        }
    }
}
