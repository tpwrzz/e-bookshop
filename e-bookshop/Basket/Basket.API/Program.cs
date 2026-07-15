using Basket.Application.Baskets.Commands;
using Basket.Application.Behaviors;
using Basket.Domain.Repositories;
using Basket.Infrastructure.Consumers;
using Basket.Infrastructure.Grpc;
using Basket.Infrastructure.Repositories;
using Bookshop.Contracts.Grpc;
using Couchbase.Extensions.DependencyInjection;
using FluentValidation;
using MassTransit;
using MediatR;
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
        .WriteTo.File("logs/basket-.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCouchbase(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("Couchbase");
        options.UserName = builder.Configuration["Couchbase:Username"];
        options.Password = builder.Configuration["Couchbase:Password"];
    });
    builder.Services.AddCouchbaseBucket<INamedBucketProvider>(
        builder.Configuration["Couchbase:BucketName"]!);

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<OrderPlacedConsumer>();

        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
            {
                h.Username(builder.Configuration["RabbitMQ:Username"]);
                h.Password(builder.Configuration["RabbitMQ:Password"]);
            });

            cfg.ConfigureEndpoints(ctx);
        });
    });
    builder.Services.AddGrpcClient<CatalogGrpcService.CatalogGrpcServiceClient>(o =>
    {
        o.Address = new Uri(builder.Configuration["Catalog:GrpcAddress"]!);
    });
    builder.Services.AddScoped<ICatalogPriceClient, CatalogPriceClient>();

    builder.Services.AddValidatorsFromAssembly(typeof(UpsertBasketCommand).Assembly);

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(UpsertBasketCommand).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    });
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Basket API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}