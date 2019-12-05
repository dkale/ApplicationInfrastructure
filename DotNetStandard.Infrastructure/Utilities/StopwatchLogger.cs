using System;

namespace DotNetStandard.Infrastructure.Utilities
{
    public class StopwatchLogger : IDisposable
    {
        private readonly string _codeBlockIdentifier;

        public StopwatchLogger(string codeBlockIdentifier)
        {
            _codeBlockIdentifier = codeBlockIdentifier;
            CodeExecutionTimeLogger.LogTimeStamp(codeBlockIdentifier);
        }

        public void Dispose()
        {
            CodeExecutionTimeLogger.LogTimeStamp(_codeBlockIdentifier);
        }
    }
}
