using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Behaviors;
using Ordering.Application.Orders.Commands;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Consumers;
using Ordering.Infrastructure.Outbox;
using Ordering.Infrastructure.Repositories;
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

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<PaymentProcessedConsumer>();
        x.AddConsumer<PaymentFailedConsumer>();

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

    builder.Services.AddHostedService<OutboxPublisherService>();

    builder.Services.AddDbContext<OrderingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDb")));

    builder.Services.AddValidatorsFromAssembly(typeof(PlaceOrderCommand).Assembly);

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(PlaceOrderCommand).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    });

    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OrderingContext>();
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
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/ordering/swagger/v1/swagger.json", "Ordering API");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ordering API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}