using System;

namespace MyCore.Logging {
    public interface ILogger {
        void Write(string message, LogLevel level = LogLevel.Error, Exception exception = null);
    }

    public enum LogLevel {
        Info,
        Warning,
        Error
    }
}