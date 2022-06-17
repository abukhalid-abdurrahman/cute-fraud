using Fraud.Concerns;

namespace Fraud.Entities.DTOs
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; } = default(T);

        public static Response<T> FromReturnResult(ReturnResult<T> returnResult) =>
            new()
            {
                Code = returnResult.ErrorCode,
                Message = returnResult.Message,
                Payload = returnResult.Result
            };
    }
}