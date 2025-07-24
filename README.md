# QuantSignalServer

量化信号服务器，用于接收、处理和转发交易信号。

## 前置条件
- .NET 7 SDK
- SQLite（数据库文件会在运行时自动创建）

## 安装
1. 克隆项目到本地
2. 运行 `dotnet restore` 恢复依赖
3. 修改 `appsettings.json` 中的 `Jwt:Key` 为安全的密钥
4. 运行 `dotnet ef database update` 初始化数据库
5. 运行 `dotnet run` 启动服务器

## 使用
1. 访问 `https://localhost:5001/swagger` 查看 API 文档
2. 访问 `https://localhost:5001/strategies` 查看和管理策略
3. 访问 `https://localhost:5001/signals` 查看信号历史

## API 端点
- POST `/api/Auth/register` - 注册用户
- POST `/api/Auth/login` - 登录获取 JWT
- POST `/api/Strategy` - 创建策略
- GET `/api/Strategy` - 获取用户策略
- PUT `/api/Strategy/{id}` - 更新策略
- DELETE `/api/Strategy/{id}` - 删除策略
- POST `/api/Signal` - 接收并转发信号
- GET `/api/Signal/history/{strategyId}` - 获取信号历史

## 注意事项
- 默认端口为 5001，可在 `Program.cs` 中修改
- 确保转发目标 URL 可访问
- 普通用户最多创建 3 个策略，VIP 用户无限制
- 每个策略最多配置 9 个转发目标
