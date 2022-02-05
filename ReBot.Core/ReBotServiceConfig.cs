using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace ReBot.Core;

public static class ReBotServiceConfig
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        services.AddDiscordGateway(_ => config["Bot:Token"]);
        services.AddResponder<PingResponder>();
    }
}