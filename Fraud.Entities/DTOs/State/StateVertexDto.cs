using System.ComponentModel.DataAnnotations;
using Fraud.Concerns.Extensions;
using Fraud.Entities.DTOs.Action;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.State
{
    public class StateVertexDto
    {
        public int? StateId { get; set; }
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Please provide state name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please choose one of state type!")] 
        public StateType StateType { get; set; }
        public string StateTypeName => StateType.GetDescription();
        [Required(ErrorMessage = "Please choose one of state action!")]
        public ActionTypeDto[] ActionTypes { get; set; }
        public int? ExpirationTime { get; set; } = 30; // Expiration time in seconds
        
        public static implicit operator Models.State(StateVertexDto stateVertexDto) => new(stateVertexDto);
    }
}