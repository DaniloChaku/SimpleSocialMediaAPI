using Dapper;
using FluentValidation;
using SocialMediaApi.Extensions;
using SocialMediaApi.Validators;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

// Configure API documentation
builder.Services.AddApiDocumentation();

var app = builder.Build();

app.ConfigureMiddleware(builder.Environment);

await app.InitializeDatabaseAsync();

app.RegisterEndpoints();

app.Run();