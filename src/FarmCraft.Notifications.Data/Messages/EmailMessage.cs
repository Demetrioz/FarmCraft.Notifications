using FarmCraft.Core.Messaging;

namespace FarmCraft.Notifications.Data.Messages
{
    public class EmailMessage : FarmCraftMessage, INotificationMessage
    {
    }

    public class EmailData
    {
        public string To { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string[] BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
