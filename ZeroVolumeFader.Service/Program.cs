// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZeroVolumeFader;
using ZeroVolumeFader.Service;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<ISystemAudio, SystemAudio>();
builder.Services.AddSingleton<Fader>();
builder.Services.AddMediatR(cfg     => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHostedService<Worker>();
builder.Logging.AddSimpleConsole().SetMinimumLevel(LogLevel.Debug);

var host = builder.Build();
host.Run();