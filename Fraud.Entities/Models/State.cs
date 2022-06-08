using System;
using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class State
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public StateType StateCode { get; set; }
        public string StateName { get; set; }
        public DateTimeOffset ExpirationTime { get; set; }
    }
}