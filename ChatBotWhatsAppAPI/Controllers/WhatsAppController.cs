using ChatBotWhatsAppAPI.Models.WhatsAppCloud;
using ChatBotWhatsAppAPI.Services.Google.Gemini;
using ChatBotWhatsAppAPI.Services.WhatsAppCloud.SendMessage;
using ChatBotWhatsAppAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotWhatsAppAPI.Controllers
{

    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : Controller
    {
        private readonly IWhatsAppCloudSendMessage _whatsAppCloudSendMessage;
        private readonly IUtilities _utilities;
        private readonly IGeminiService _gemini;

        public WhatsAppController(IWhatsAppCloudSendMessage whatsAppCloudSendMessage, IUtilities utilities, IGeminiService gemini)
        {
            _whatsAppCloudSendMessage = whatsAppCloudSendMessage;
            _utilities = utilities;
            _gemini = gemini;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = "59895448562",
                type = "text",
                text = new
                {
                    preview_url = false,
                    body = "Este es un nuevo mensaje"
                }
            };

            var result = await _whatsAppCloudSendMessage.Execute(data);

            return Ok("ok sample");
        }

        [HttpGet]
        public IActionResult VerifyToken()
        {
            string AccesToken = "asjkdhasjkhas8d98asdhsoaidj09";
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if(challenge != null && token != null && token == AccesToken)
            {
                return Ok(challenge);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsAppCloudModel body)
        {
            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];
                if(Message != null)
                {
                    var userNumber = Message.From;
                    var userText = GetUserText(Message);

                    List<object> listObjectMessage = new List<object>();

                    #region sin chatgpt
                    //if(userText.ToUpper().Contains("HOLA"))
                    //{
                    //    var objectMessage = _utilities.TextMessage("Hola, soy el asistente virtual de FSClock 😀", userNumber);
                    //    var objectMessage2 = _utilities.TextMessage("¿Cómo puedo ayudarte hoy?", userNumber);
                    //    listObjectMessage.Add(objectMessage);
                    //    listObjectMessage.Add(objectMessage2);
                    //}
                    //else
                    //{
                    //    var objectMessage = _utilities.TextMessage("Lo siento, no puedo entenderte 🥺", userNumber);
                    //    listObjectMessage.Add(objectMessage);
                    //}
                    #endregion

                    var responseGemini = await _gemini.Execute(userText);
                    var objectMessage = _utilities.TextMessage(responseGemini, userNumber);
                    listObjectMessage.Add(objectMessage);

                    foreach (var item in listObjectMessage)
                    {
                        await _whatsAppCloudSendMessage.Execute(item);
                    }
                }

                return Ok("EVENT_RECEIVED");
            }
            catch (Exception ex) 
            {

                return Ok("EVENT_RECEIVED");
            }
        }

        private string GetUserText(Message message)
        {
            string TypeMessage = message.Type;

            if(TypeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }
            else if(TypeMessage.ToUpper() == "INTERACTIVE")
            {
                string interacticeType = message.Interactive.Type;

                if(interacticeType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                }else if(interacticeType.ToUpper() == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
