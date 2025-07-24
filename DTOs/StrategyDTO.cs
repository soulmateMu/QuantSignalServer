namespace QuantSignalServer.DTOs
{
    public class StrategyDTO
    {
        public required string Name { get; set; }
        public List<string>? ForwardTargets { get; set; } // 转发目标 URL 列表
    }
}