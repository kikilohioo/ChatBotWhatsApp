namespace ChatBotWhatsAppAPI.Services.WhatsAppCloud.SendMessage
{
    public interface IWhatsAppCloudSendMessage
    {
        Task<bool> Execute(object model);
    }
}
