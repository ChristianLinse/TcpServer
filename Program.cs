using TcpServer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IProtocol, WhateverProtocol>();
builder.Services.AddHostedService<Worker>();
await builder.Build().RunAsync();
