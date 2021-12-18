using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Results;
using Remora.Results;

var cancellationSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellationSource.Cancel();
};

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

var token = config["Bot:Token"];

var services = new ServiceCollection()
    .AddLogging(cfg => cfg.AddConsole())
    .AddDiscordGateway(_ => config["Bot:Token"])
    .BuildServiceProvider();

var gatewayClient = services.GetRequiredService<DiscordGatewayClient>();
var runResult = await gatewayClient.RunAsync(cancellationSource.Token);

var log = services.GetRequiredService<ILogger<Program>>();

if (!runResult.IsSuccess)
{
    switch (runResult.Error)
    {
        case ExceptionError exe:
            {
                log.LogError(exe.Exception, "Exception during gateway connection: {ExceptionMessage}", exe.Message);
                break;
            }
        case GatewayWebSocketError:
        case GatewayDiscordError:
            {
                log.LogError("Gateway error: {Message}", runResult.Error.Message);
                break;
            }
        default:
            {
                log.LogError("Unknown error: {Message}", runResult.Error.Message);
                break;
            }
    }
}


