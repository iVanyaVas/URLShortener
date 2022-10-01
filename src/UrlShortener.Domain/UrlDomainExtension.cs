using System;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using UrlShorteneer.Domain.Database;

using UrlShortener.Domain.Commands;

namespace UrlShorteneer.Domain;

public static class UrlDomainExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> dbOptionsAction)
    {
        services.AddMediatR(typeof(CreateUrlCommand));
        services.AddDbContext<UrlDbContext>(dbOptionsAction);

        return services;

    }
}