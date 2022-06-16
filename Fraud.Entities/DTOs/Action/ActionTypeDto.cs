using System.ComponentModel.DataAnnotations;
using Fraud.Concerns.Extensions;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.Action
{
    public class ActionTypeDto
    {
        [Required]
        public int ActionId { get; set; }

        [Required(ErrorMessage = "Please choose one of state action!")]
        public ActionType ActionCode { get; set; }
        public string ActionName => ActionCode.GetDescription();
    }
}