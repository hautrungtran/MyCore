using System;
using NLog;

namespace MyCore.Logging {
    public class DefaultLogger : ILogger {
        private readonly Logger _logger;
        public DefaultLogger(string name) {
            _logger = LogManager.GetLogger(name);
        }
        public void Write(string message, LogLevel level = LogLevel.Error, Exception exception = null) {
            switch (level) {
                case LogLevel.Info:
                    _logger.InfoException(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.WarnException(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.ErrorException(message, exception);
                    break;
            }
        }
    }
}