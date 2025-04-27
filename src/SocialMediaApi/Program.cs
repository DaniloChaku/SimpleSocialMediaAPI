using SocialMediaApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<MigrationService>();

var app = builder.Build();

await InitializeDatabase(app);

app.Run();

static async Task InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
    await migrationService.InitializeAsync();
}