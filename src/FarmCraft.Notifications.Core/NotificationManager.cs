using Akka.Actor;
using Azure.Messaging.ServiceBus;
using FarmCraft.Notifications.Data.Messages;
using FarmCraft.Core.Actors;
using FarmCraft.Core.Messaging;
using Newtonsoft.Json;

namespace FarmCraft.Notifications.Core
{
    public class NotificationManager : FarmCraftManager
    {
        private readonly IActorRef _handler;

        public NotificationManager(IServiceProvider provider) : base(provider)
        {
            _handler = Context.ActorOf(
                Props.Create(() => new NotificationHandler(provider, HandleMessage, HandleError)),
                "handler"
            );
        }

        public async Task HandleMessage(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();
                FarmCraftMessage message = JsonConvert.DeserializeObject<FarmCraftMessage>(body);
                INotificationMessage typedMessage = CreateTypedMessage(message);

                await _handler.Ask(typedMessage, TimeSpan.FromSeconds(5));
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                var pause = true;
            }
        }

        public Task HandleError(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private INotificationMessage CreateTypedMessage(FarmCraftMessage message)
        //private INotificationMessage CreateTypedMessage<T>(T type, FarmCraftMessage message) 
        //    where T : Type
        {
            var stringData = JsonConvert.SerializeObject(message);
            INotificationMessage typedMessage;

            if (message.MessageType == typeof(EmailMessage))
                typedMessage = JsonConvert.DeserializeObject<EmailMessage>(stringData);

            else if (message.MessageType == typeof(TextMessage))
                typedMessage = JsonConvert.DeserializeObject<TextMessage>(stringData);


            else if (message.MessageType == typeof(ChatMessage))
                typedMessage = JsonConvert.DeserializeObject<ChatMessage>(stringData);

            else
                throw new Exception("Improper message type..");

            return typedMessage;
        }
    }
}
