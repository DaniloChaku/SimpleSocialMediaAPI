using Dapper;
using FluentValidation;
using SocialMediaApi.Data;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Endpoints;
using SocialMediaApi.Validators;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<MigrationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Social Media API",
        Version = "v1",
        Description = "A simple Minimal API for a social media app.",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API V1");
    });
}

await InitializeDatabase(app);

UserEndpoints.Map(app);
PostEndpoints.Map(app);

app.Run();

static async Task InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
    await migrationService.InitializeAsync();
}