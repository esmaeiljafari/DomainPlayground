using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Infrastructure.Localization;
using DomainPlayground.Infrastructure.Localization.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace DomainPlayground.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddLocalization();

        services.AddScoped<IErrorLocalizer, ErrorLocalizer>();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture("fa")
                .AddSupportedCultures("fa", "en")
                .AddSupportedUICultures("fa", "en");
        });

        return services;
    }
}