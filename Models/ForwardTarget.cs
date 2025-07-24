namespace QuantSignalServer.Models
{
    public class ForwardTarget
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public int StrategyId { get; set; }
    }
}