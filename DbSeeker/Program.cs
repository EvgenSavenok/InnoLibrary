using DBSeeder.ConnectionFactories;
using DBSeeder.Contracts;
using DbSeeder.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5001"); 

builder.Services.AddScoped<IConnectionFactory, BookingConnectionFactory>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var seeder = new SeedService(scope.ServiceProvider.GetRequiredService<IConnectionFactory>());
await seeder.SeedAuthorsAsync(10);
await seeder.SeedBooksAsync(10);
await seeder.SeedReservationsAsync(10);

