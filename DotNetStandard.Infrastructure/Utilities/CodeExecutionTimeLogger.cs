using DotNetStandard.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetStandard.Infrastructure.Utilities
{
    public static class CodeExecutionTimeLogger
    {
        private static readonly Stack<string> _codeInvocationStack;
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static ILogger _logger;

        static CodeExecutionTimeLogger()
        {
            _codeInvocationStack = new Stack<string>();
        }

        /// <summary>
        /// Method allows to measure the time elapsed for execution of a code block
        /// </summary>
        /// <param name="codeBlockIdentifier">A name/identifier user for identifying the start and end of code block.</param>
        /// <returns>Message to log the TimeStamp and time elapsed</returns>
        internal static string LogTimeStamp(string codeBlockIdentifier)
        {
            string logMessage;

            if (_codeInvocationStack.Count == 0)
            {
                logMessage = Environment.NewLine + $"Code-Execution-Stopwatch: Start time for \"{codeBlockIdentifier}\" = " + DateTime.Now.TimeOfDay;
                _logger.LogInformation(logMessage);
                _stopwatch.Start();
                _codeInvocationStack.Push(codeBlockIdentifier);
            }
            else
            {
                logMessage = Environment.NewLine + $"Code-Execution-Stopwatch: End time for \"{codeBlockIdentifier}\"  = " + DateTime.Now.TimeOfDay;
                _stopwatch.Stop();
                //var timeElapsed = $"Time taken for executing code/method \"{codeBlockIdentifier}\" = {_stopwatch.Elapsed:c}";
                var timeElapsed = $"Code-Execution-Stopwatch: Total execution time for \"{codeBlockIdentifier}\" = {_stopwatch.Elapsed.ToFriendlyDisplay()}";
                logMessage = logMessage + Environment.NewLine + timeElapsed;
                _logger.LogInformation(logMessage);
                _stopwatch.Reset();
                _codeInvocationStack.Pop();
            }

            return logMessage;
        }

        /// <summary>
        /// Returns an instance of disposable StopwatchLogger which will write a long on object instantiation and disposal when used in a 'using' block
        /// </summary>
        /// <param name="isEnabled">Determines if log will be written</param>
        /// <param name="codeBlockIdentifier"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static StopwatchLogger LogCodeExecutionTime(ILogger logger, bool isEnabled = false, [CallerMemberName]string codeBlockIdentifier = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!isEnabled)
                return null;

            if (logger == null)
                return null;

            _logger = logger;

            var callingMethodName = new StackTrace().GetFrame(1).GetMethod().Name;

            var codeIdentifier = codeBlockIdentifier.EqualsIgnoreCase(callingMethodName) ? $"Method:{callingMethodName}" : $"Block:{codeBlockIdentifier}";

            return new StopwatchLogger(codeIdentifier);
        }
    }
}
