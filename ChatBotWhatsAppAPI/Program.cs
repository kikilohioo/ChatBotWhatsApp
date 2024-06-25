using ChatBotWhatsAppAPI.Services.Google.Gemini;
using ChatBotWhatsAppAPI.Services.WhatsAppCloud.SendMessage;
using ChatBotWhatsAppAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IWhatsAppCloudSendMessage, WhatsAppCloudSendMessage>();
builder.Services.AddSingleton<IUtilities, Utilities>();
builder.Services.AddSingleton<IGeminiService, GeminiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
