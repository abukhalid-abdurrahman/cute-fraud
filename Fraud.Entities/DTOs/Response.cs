namespace Fraud.Entities.DTOs
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; } = default(T);
    }
}