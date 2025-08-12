using Infrastructure.Extensions;
using Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.ApplyMigrations();

app.Run();