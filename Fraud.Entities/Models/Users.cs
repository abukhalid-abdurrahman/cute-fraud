using System;

namespace Fraud.Entities.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ApiKey { get; set; }
        public string CallBack { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
    }
}