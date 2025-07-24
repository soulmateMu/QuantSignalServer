namespace QuantSignalServer.Models
{
    public class Strategy
    {
        public int Id { get; set; } // 服务器维护的主键
        public required string Name { get; set; } // 用户可见的策略名称
        public int UserId { get; set; } // 关联用户主键，int 类型
        public User? User { get; set; }
        public List<ForwardTarget> ForwardTargets { get; set; } = new();
    }
}