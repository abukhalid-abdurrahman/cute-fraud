namespace Fraud.Entities.Models
{
    public class EventHistory
    {
        public int Id { get; set; }
        public string OrderExternalRef { get; set; }
        public int ScenarioId { get; set; }
        public int StateId { get; set; }
        public int EventId { get; set; }
    }
}