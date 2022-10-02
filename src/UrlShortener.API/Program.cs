using System;
using System.IO;
using System.Reflection;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using UrlShorteneer.API.Configuration;
using UrlShorteneer.Domain;
using UrlShorteneer.Domain.Database;

using UrlShortener.Domain.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Url Shortener API",
        Description = "Web API to generate short links that uses redirect to their origin route"
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddHealthChecks().AddNpgSql((sp) =>
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    return configuration.CurrentValue.ConnectionString;

}, timeout: TimeSpan.FromSeconds(3));



builder.Services.Configure<AppConfiguration>(builder.Configuration);
builder.Services.AddDomainServices((serviceProvider, options)=>
{
    var configuration = serviceProvider.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    options.UseNpgsql(configuration.CurrentValue.ConnectionString).UseLoggerFactory(loggerFactory);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    }
    );
}


app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }