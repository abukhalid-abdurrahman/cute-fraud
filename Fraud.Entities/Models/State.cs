using System;
using Fraud.Entities.DTOs.State;
using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class State
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public StateType StateCode { get; set; }
        public string StateName { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public int ExpirationTime { get; set; }

        public State(StateRequestDto stateRequestDto)
        {
            UserId = stateRequestDto.UserId;
            StateCode = stateRequestDto.StateType;
            StateName = stateRequestDto.Name;
            ExpirationTime = stateRequestDto.ExpirationTime;
        }
    }
}