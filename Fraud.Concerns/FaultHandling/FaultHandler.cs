using System;
using System.Collections.Generic;
using Serilog;

namespace Fraud.Concerns.FaultHandling
{
    public static class FaultHandler
    {
        public static void HandleError<T>(
            ref ReturnResult<T> returnResult, 
            Exception exception)
        {
            string errorMessageTemplate = 
                "Error was occured while executing request! Reason: {0}, {1}";
            // Setup ReturnResult instance
            returnResult.IsSuccessfully = false;
            returnResult.Exception = exception;
            returnResult.Message = exception?.Message;
            returnResult.DetailedMessage = exception?.StackTrace;

            var errorMessage = string.Format(errorMessageTemplate, exception?.Message, exception?.StackTrace);
            returnResult.ErrorMessages = new List<string>()
            {
                errorMessage
            };
            
            Log.Logger.Error(errorMessage);
        }
        
        public static void HandleError<T>(
            ref ReturnResult<T> returnResult, 
            Exception exception,
            string message,
            string detailedMessage)
        {
            string errorMessageTemplate = 
                "Error was occured while executing request! Reason: {0}, {1}";
            var errorMessage = string.Format(errorMessageTemplate, message, detailedMessage);

            // Setup ReturnResult instance
            returnResult.IsSuccessfully = false;
            returnResult.Exception = exception;
            returnResult.Message = exception?.Message;
            returnResult.DetailedMessage = exception?.StackTrace;

            returnResult.ErrorMessages = new List<string>()
            {
                errorMessage
            };
            
            Log.Logger.Error(errorMessage);
        }
        
        public static void HandleError<T>(
            ref ReturnResult<T> returnResult, 
            string message)
        {
            string errorMessageTemplate = 
                "Error was occured while executing request! Reason: {0}";
            var errorMessage = string.Format(errorMessageTemplate, message);

            // Setup ReturnResult instance
            returnResult.IsSuccessfully = false;
            returnResult.Message = message;
            returnResult.ErrorMessages = new List<string>()
            {
                errorMessage
            };
            
            Log.Logger.Error(errorMessage);
        }

        public static void HandleWarning<T>(
            ref ReturnResult<T> returnResult,
            string message,
            string detailedMessage)
        {
            string errorMessageTemplate = 
                "Warning! Reason: {0}, {1}";
            var warningMessage = string.Format(errorMessageTemplate, message, detailedMessage);

            // Setup ReturnResult instance
            returnResult.IsSuccessfully = true;
            returnResult.Message = message;
            returnResult.DetailedMessage = detailedMessage;

            Log.Logger.Warning(warningMessage);
        }
    }
}