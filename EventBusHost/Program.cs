using Autofac;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using EventBus.EventBusServiceBus;
using Producer.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Configuration.GetValue<bool>("AzureServiceBusEnabled"))
{
    builder.Services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
    {
        var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
        string subscriptionName = builder.Configuration["SubscriptionClientName"];

        return new EventBusServiceBus(serviceBusPersisterConnection, logger,
            eventBusSubcriptionsManager, iLifetimeScope, subscriptionName);
    });

}
else
{
    builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
    {
        var subscriptionClientName = builder.Configuration["SubscriptionClientName"];
        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        var retryCount = 5;
        if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
        {
            retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
        }

        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
    });
}

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
builder.Services.AddSingleton<IPublishEventController, PublishEventController>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();