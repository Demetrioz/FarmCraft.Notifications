using Akka.Actor;
using Azure.Messaging.ServiceBus;
using FarmCraft.Core.Actors;
using FarmCraft.Core.Messaging;
using FarmCraft.Core.Services.Messaging;
using FarmCraft.Notifications.Data.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FarmCraft.Notifications.Core
{
    public class NotificationHandler : FarmCraftActor
    {
        private readonly ServiceBusProcessor _processor;

        public NotificationHandler(
            IServiceProvider provider, 
            Func<ProcessMessageEventArgs, Task> messageHandler,
            Func<ProcessErrorEventArgs, Task> errorHandler
        ) : base(provider)
        {
            Receive<EmailMessage>(message => SendEmail(message));
            Receive<TextMessage>(message => SendText(message));
            Receive<ChatMessage>(message => SendChat(message));

            using (var scope = provider.CreateScope())
            {
                var client = scope.ServiceProvider.GetService<ServiceBusClient>();
                var options = scope.ServiceProvider.GetService<IOptions<ConsumerOptions>>();
                _processor = client.CreateProcessor(
                    options.Value.Queue,
                    new ServiceBusProcessorOptions 
                    { 
                        AutoCompleteMessages = false 
                    }
                );
                _processor.ProcessMessageAsync += messageHandler;
                _processor.ProcessErrorAsync += errorHandler;
                _processor.StartProcessingAsync().Wait();
            }

            var pause = true;
        }

        //public Task HandleMessage(ProcessMessageEventArgs args)
        //{
        //    var self = Self;

        //    string body = args.Message.Body.ToString();
        //    FarmCraftMessage message = JsonConvert.DeserializeObject<FarmCraftMessage>(body);
        //    self.Tell(message);
        //    return Task.CompletedTask;
        //}

        //public Task HandleError(ProcessErrorEventArgs args)
        //{
        //    Console.WriteLine(args.Exception.ToString());
        //    return Task.CompletedTask;
        //}

        //private void HandleEmail(EmailMessage message)
        //{
        //    SendEmail(message).ContinueWith(result =>
        //    {
        //        Console.WriteLine("");
        //    });
        //}

        //private void HandleText(TextMessage message)
        //{

        //}

        //private void HandleChat(ChatMessage message)
        //{

        //}

        private async Task SendEmail(EmailMessage email)
        {
            Console.WriteLine("Sending Email");
            Sender.Tell(true);
        }

        private async Task SendText(TextMessage text)
        {
            Console.WriteLine("Sending Text");
            Sender.Tell(true);
        }

        private async Task SendChat(ChatMessage chat)
        {
            Console.WriteLine("Sending Chat");
            Sender.Tell(true);
        }
    }
}
