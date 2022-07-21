using Azure.Messaging.ServiceBus;
using FarmCraft.Core.Services.Messaging;
using FarmCraft.Notifications.Core;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // TODO:
        // Use ENV first
        // fallback to appsettings.json
        // fallback to error

        services.Configure<ConsumerOptions>(options =>
        {
            options.Host = "Endpoint=sb://sb-prototyping.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=voTdcylNVXgYjGpOHEuto0y2G7ZJkwmM0y7NgcQ9rko=";
            options.Queue = "sbq-prototype";
        });

        services.AddSingleton(provider =>
        {
            return new ServiceBusClient("Endpoint=sb://sb-prototyping.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=voTdcylNVXgYjGpOHEuto0y2G7ZJkwmM0y7NgcQ9rko=");
        });

        services.AddHostedService<NotificationService>();
    })
    .Build();

await host.RunAsync();
