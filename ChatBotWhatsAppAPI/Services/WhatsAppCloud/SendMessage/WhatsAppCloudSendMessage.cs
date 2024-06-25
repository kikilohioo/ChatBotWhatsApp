using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace ChatBotWhatsAppAPI.Services.WhatsAppCloud.SendMessage
{
    public class WhatsAppCloudSendMessage: IWhatsAppCloudSendMessage
    {
        private readonly IConfiguration _configuration;

        public WhatsAppCloudSendMessage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string BaseUrl = _configuration.GetValue<string>("WhatsAppCloudAPI:BaseUrl");
                string ApiVersion = _configuration.GetValue<string>("WhatsAppCloudAPI:ApiVersion");
                string PhoneNumberId = _configuration.GetValue<string>("WhatsAppCloudAPI:PhoneNumberId");
                string BearerToken = _configuration.GetValue<string>("WhatsAppCloudAPI:BearerToken");
                string uri = $"{BaseUrl}/{ApiVersion}/{PhoneNumberId}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {BearerToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
