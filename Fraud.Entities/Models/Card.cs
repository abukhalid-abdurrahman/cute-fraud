using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class Card
    {
        public string CardToken { get; set; }
        public CardState CardState { get; set; } = CardState.Default;
    }
}