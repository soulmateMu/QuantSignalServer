namespace QuantSignalServer.Models
{
    public class Strategy
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public required List<ForwardTarget> ForwardTargets { get; set; }
    }
}