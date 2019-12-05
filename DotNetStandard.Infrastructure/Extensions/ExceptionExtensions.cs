using Newtonsoft.Json.Linq;
using System;

namespace DotNetStandard.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetInnerExceptionMessage(this Exception exception)
        {
            var previousException = exception;
            while (exception.InnerException != null)
            {
                previousException = exception;
                exception = exception.InnerException;
            }

            string errorMessage;
            try
            {
                var obj = JObject.Parse(exception.Message);
                var token = exception.Message.Contains("ExceptionMessage") ? "ExceptionMessage" : "Message";
                errorMessage = obj.SelectToken(token).ToString();
            }
            catch (Exception)
            {
                errorMessage = previousException.Message;
            }

            return errorMessage;
        }
    }
}
