// See https://aka.ms/new-console-template for more information

using Azure.Messaging.ServiceBus;
using FarmCraft.Notifications.Data.Messages;
using Newtonsoft.Json;

const int messageCount = 2;

//var options = Options.Create(pubOps);

ServiceBusClient client = new ServiceBusClient("Endpoint=sb://sb-prototyping.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=voTdcylNVXgYjGpOHEuto0y2G7ZJkwmM0y7NgcQ9rko=");
ServiceBusSender sender = client.CreateSender("sbq-prototype");

for(int i = 0; i < messageCount; i++)
{
    EmailMessage message = new EmailMessage
    {
        Timestamp = DateTime.UtcNow,
        MessageType = typeof(EmailMessage),
        Data = new EmailData
        {
            To = "Bill@test.com",
            FromName = "Tester",
            FromEmail = "Test@test.com",
            Subject = "Test Email",
            Body = "This is a test. Did it work?"
        }
    };

    string stringMessage = JsonConvert.SerializeObject(message);
    sender.SendMessageAsync(new ServiceBusMessage(stringMessage)).Wait();
}

Console.WriteLine("Completed...");
