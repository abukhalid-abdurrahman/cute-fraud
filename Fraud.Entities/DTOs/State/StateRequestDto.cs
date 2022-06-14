using System.ComponentModel.DataAnnotations;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.State
{
    public class StateRequestDto
    {
        public int UserId { get; set; }
        public StateRequestDto PreviousState { get; set; }
        public StateRequestDto[] NextStates { get; set; }
        [Required(ErrorMessage = "Please provide state name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please choose one of state type!")] 
        public StateType StateType { get; set; }
        [Required(ErrorMessage = "Please choose one of state action!")]
        public ActionType[] ActionTypes { get; set; }
        [Required(ErrorMessage = "Please provide state event name!")]
        public EventType EventType { get; set; }
        public int ExpirationTime { get; set; } = 30; // Expiration time in seconds
        
        public static implicit operator Models.State(StateRequestDto stateRequestDto) => new(stateRequestDto);
    }
}