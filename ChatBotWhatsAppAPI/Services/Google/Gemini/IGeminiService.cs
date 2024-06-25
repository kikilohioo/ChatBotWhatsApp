namespace ChatBotWhatsAppAPI.Services.Google.Gemini
{
    public interface IGeminiService
    {
        Task<string> Execute(string userText);
    }
}
