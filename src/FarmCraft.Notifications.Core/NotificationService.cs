using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using FarmCraft.Core.Actors;
using FarmCraft.Core.Messaging;
using FarmCraft.Core.Services.Messaging;
using Microsoft.Extensions.Options;

namespace FarmCraft.Notifications.Core
{
    public class NotificationService : FarmCraftCore<NotificationManager>
    {
        //private readonly IMessageConsumer _messageConsumer;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IServiceProvider provider, 
            ILogger<NotificationService> logger
            //IOptions<ConsumerOptions> options
        ) : base(provider)
        {
            _logger = logger;
            //_messageConsumer = new ServiceBusConsumer(options, logger);
            //_messageConsumer.Register<>((message) => HandleMessage(message));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var hocon = ConfigurationFactory.ParseString(await File.ReadAllTextAsync("actor.conf", stoppingToken));
            var hocon = ConfigurationFactory.Default();
            var bootStrap = BootstrapSetup.Create().WithConfig(hocon);
            var di = DependencyResolverSetup.Create(_serviceProvider);
            var actorSystemSetup = bootStrap.And(di);
            _actorSystem = ActorSystem.Create("FarmCraftCore", actorSystemSetup);
            _root = _actorSystem.ActorOf(Props.Create(() => new NotificationManager(_serviceProvider)), "FarmCraftManager");
        }

        //private void HandleMessage(FarmCraftMessage message)
        //{
        //    _root.Tell(message);
        //}
    }
}