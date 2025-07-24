namespace QuantSignalServer.DTOs
{
    public class StrategyDTO
    {
        public required string Name { get; set; }
        public required List<string> ForwardTargets { get; set; }
    }
}