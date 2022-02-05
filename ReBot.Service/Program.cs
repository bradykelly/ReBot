using ReBot.Core;
using ReBot.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.AddConsole();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 
        builder.AddUserSecrets<Program>();
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration config = hostContext.Configuration;
        services.AddHostedService<Worker>();
        ReBotServiceConfig.Configure(services, config);
    })
    .Build();

await host.RunAsync();
