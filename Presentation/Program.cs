using Infrastructure.Extensions;
using Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureBookingSqlContext(builder.Configuration);
builder.Services.ConfigureUserSqlContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureRateLimiting();
builder.Services.ConfigureSwagger();
builder.Services.AddValidators();

builder.WebHost.UseUrls("http://0.0.0.0:5000"); 
builder.Services.AddMediatR(cfg =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    cfg.RegisterServicesFromAssemblies(assemblies);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "InnoLibrary API");
});

app.UseRouting();

app.MapControllers();
app.MapRazorPages();

app.ApplyMigrations();

app.Run();