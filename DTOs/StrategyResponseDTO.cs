namespace QuantSignalServer.DTOs
{
    public class StrategyResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public required List<string> ForwardTargets { get; set; }
    }
}