namespace Fraud.Entities.Models
{
    public class Scenario
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Rule { get; set; }
    }
}