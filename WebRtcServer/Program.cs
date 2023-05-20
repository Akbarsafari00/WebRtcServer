using WebRtcServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSingleton<UsersStorage>(new UsersStorage());
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
    o.MaximumReceiveMessageSize = 102400000;
});

var app = builder.Build();



app.UseRouting();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<VoiceCallHub>("/hub/voiceCall");
});

app.Run();
