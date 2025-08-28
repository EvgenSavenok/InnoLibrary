using Application.Contracts.Repository.Booking;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Extensions;

public static class DbExtension
{
    public static void ConfigureBookingSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("bookingConnection")));
    }
    
    public static void ConfigureUserSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("userConnection")));
    }

    public static void ConfigureUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var bookingContext = scope.ServiceProvider.GetRequiredService<BookingContext>();
        bookingContext.Database.Migrate();
        
        var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
        userContext.Database.Migrate();
    }
}