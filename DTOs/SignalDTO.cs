namespace QuantSignalServer.DTOs
{
    public class SignalDTO
    {
        public required string StrategyName { get; set; } // 改为引用 Strategy.Name
        public required string StockName { get; set; }
        public required string StockCode { get; set; }
        public required string Direction { get; set; } // "Buy" 或 "Sell"
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }
}