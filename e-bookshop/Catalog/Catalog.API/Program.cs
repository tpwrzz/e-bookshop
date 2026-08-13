using Bookshop.SharedKernel;
using Catalog.Application.Behaviors;
using Catalog.Application.Books.Commands;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, services, config) => config
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        .WriteTo.File("logs/catalog-.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));
    builder.Services.AddGrpc();
    builder.Services.AddControllers(); builder.Services.AddSwaggerGen(options =>
    {
        options.AddServer(new Microsoft.OpenApi.OpenApiServer { Url = "/catalog", Description = "Via Gateway" });
        options.AddServer(new Microsoft.OpenApi.OpenApiServer { Url = "/", Description = "Direct" });
    });
    builder.Services.AddDbContext<CatalogContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb")));

    builder.Services.AddValidatorsFromAssembly(typeof(CreateBookCommand).Assembly);

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(CreateBookCommand).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    });

    builder.Services.AddScoped<IBookRepository, BookRepository>();
    builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    builder.WebHost.ConfigureKestrel(options =>
    {
        // REST API + Swagger
        options.ListenAnyIP(8080, o =>
        {
            o.Protocols = HttpProtocols.Http1;
        });

        // gRPC
        options.ListenAnyIP(8081, o =>
        {
            o.Protocols = HttpProtocols.Http2;
        });
    });
    var app = builder.Build();
    app.Use(async (context, next) =>
    {
        Log.Information("Incoming headers: {Headers}",
            string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}")));
        await next();
    });
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();
        await db.Database.MigrateAsync();
    }
    app.UseSerilogRequestLogging(opts =>
    {
        opts.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Keep the schema request beneath /catalog/swagger when the API is proxied.
            options.SwaggerEndpoint("./v1/swagger.json", "Catalog API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Catalog API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
