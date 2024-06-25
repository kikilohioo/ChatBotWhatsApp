using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model.Response;
using Microsoft.Extensions.Configuration;

namespace ChatBotWhatsAppAPI.Services.Google.Gemini
{
    public class GeminiService: IGeminiService
    {

        private readonly IConfiguration _configuration;

        public GeminiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Execute(string userText)
        {
            try
            {
                var geminiConfig = new GoogleGeminiConfig()
                {
                    ApiKey = _configuration.GetValue<string>("GeminiAPI:ApiKey")
                };

                var geminiClient = new GeminiClient(geminiConfig);

                var response = await geminiClient.TextPrompt(userText);
                var auxResponse = response.Candidates[0]?.Content?.Parts[0]?.Text;

                if(auxResponse != null)
                {
                    return auxResponse;
                }
                return "Ocurrió algo inesperado, por favor intentelo nuevamente más tarde.";
            }
            catch(Exception ex) 
            {
                return "Ocurrió algo inesperado, por favor intentelo nuevamente más tarde.";
            }
        }
    }
}
