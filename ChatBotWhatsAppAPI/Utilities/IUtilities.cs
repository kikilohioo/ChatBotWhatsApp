namespace ChatBotWhatsAppAPI.Utilities
{
    public interface IUtilities
    {
        object TextMessage(string message, string number);
        object ImageMessage(string url, string number);
        object AudioMessage(string url, string number);
        object VideoMessage(string url, string number);
        object DocumentMessage(string url, string number);
        object LocationMessage(string lat, string lon, string title, string addr, string number);
        object ButtonsMessage(string number);
    }
}
