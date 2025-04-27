using SocialMediaApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();

var app = builder.Build();

app.Run();
