﻿using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using zip2vd.cli;
using zip2vd.core;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(config =>
{
    config.MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}");
});

builder.Services.Configure<FileVdOptions>(options =>
{
    options.FilePath = "D:\\test1.zip";
    options.MountPath = "R:\\";
});
builder.Services.AddSingleton<IVdService, FileVdService>();
builder.Services.AddHostedService<FsHostedService>();

using IHost host = builder.Build();
await host.RunAsync();