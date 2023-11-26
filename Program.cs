using TcpServer;

using MassTransit;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context,cfg) =>
    {
        cfg.Host("localhost");
        cfg.ConfigureEndpoints(context);
    });

    config.AddConsumers(Assembly.GetExecutingAssembly());
});

builder.Services.AddSingleton< Dictionary<int, IProtocol>>();
builder.Services.AddScoped<IProtocol, WhateverProtocol>();
builder.Services.AddHostedService<Worker>();
await builder.Build().RunAsync();
