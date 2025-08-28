using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class PolicyExtension
{
    public static void AddAuthorizationPolicy(this IServiceCollection services) =>
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireRole("Administrator"));
            options.AddPolicy("User", policy =>
                policy.RequireRole("User"));
            options.AddPolicy("AdminOrUser", policy =>
                policy.RequireRole("Administrator", "User"));
        });
}