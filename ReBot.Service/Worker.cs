using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Results;
using Remora.Results;

namespace ReBot.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DiscordGatewayClient _gatewayClient;

    public Worker(DiscordGatewayClient gatewayClient, ILogger<Worker> logger)
    {
        _gatewayClient = gatewayClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running gateway client at: {time}", DateTimeOffset.Now);
            var runResult = await _gatewayClient.RunAsync(stoppingToken);
            if (!runResult.IsSuccess)
            {
                switch (runResult.Error)
                {
                    case ExceptionError exe:
                    {
                        _logger.LogError(exe.Exception, "Exception during gateway connection: {ExceptionMessage}", exe.Message);
                        break;
                    }
                    case GatewayWebSocketError:
                    case GatewayDiscordError:
                    {
                        _logger.LogError("Gateway error: {Message}", runResult.Error.Message);
                        break;
                    }
                    default:
                    {
                        _logger.LogError("Unknown error: {Message}", runResult.Error.Message);
                        break;
                    }
                }
            }            
        }
    }
}
