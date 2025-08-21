using Application.Validation.Booking;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ValidationExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthorValidator>();
    }
}