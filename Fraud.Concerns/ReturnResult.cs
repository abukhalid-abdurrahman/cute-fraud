using System;
using System.Collections.Generic;

namespace Fraud.Concerns
{
    public struct ReturnResult<T>
    {
        public T Result { get; set; }
        public int ErrorCode { get; set; }
        public bool IsSuccessfully { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public Exception Exception { get; set; }
        public List<string> ErrorMessages { get; set; }

        public static ReturnResult<T> SuccessResult(T result = default) => new ReturnResult<T>()
        {
            ErrorCode = 1000,
            Result = result,
            IsSuccessfully = true,
            Message = "Operation completed successfully!",
            DetailedMessage = string.Empty,
            Exception = null,
            ErrorMessages = null
        };
        
        public static ReturnResult<T> FailResult(
            T result = default, 
            int errorCode = 911,
            string detailedMessage = null, 
            Exception exception = null,
            List<string> errorMessages = null) => new ReturnResult<T>()
        {
            Result = result,
            ErrorCode = errorCode,
            IsSuccessfully = false,
            Message = "Operation failed!",
            DetailedMessage = detailedMessage,
            Exception = exception,
            ErrorMessages = errorMessages
        };
    }
}