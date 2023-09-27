using System.Diagnostics;
using CommandLine;
using Microsoft.Extensions.Logging;
using ZeroVolumeFader;

var sw = new Stopwatch();
sw.Start();
// var builder = Host.CreateApplicationBuilder();
// builder.Services.AddSingleton<ISystemAudio, SystemAudio>();
// builder.Services.AddSingleton<Fader>();
// builder.Logging.AddSimpleConsole().SetMinimumLevel(LogLevel.Debug);

// var builder = Host.CreateDefaultBuilder();
// builder.ConfigureServices(collection => collection
//     .AddSingleton<ISystemAudio, SystemAudio>()
//     .AddSingleton<Fader>() );
// builder.ConfigureLogging(loggingBuilder => loggingBuilder.AddSimpleConsole().SetMinimumLevel(LogLevel.Debug));

// var host = builder.Build();
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddFilter("ZeroVolumeFader", LogLevel.Debug).AddConsole();
});

await Parser.Default.ParseArguments<FadeOptions>(args).WithParsedAsync(RunAsync);
sw.Stop();
Console.WriteLine($"Elapsed: {sw.Elapsed.TotalSeconds}");

return;

async Task RunAsync(FadeOptions o)
{
    // var fader = host.Services.GetRequiredService<Fader>();
    var fader = new Fader(new SystemAudio(), loggerFactory.CreateLogger<Fader>());
    await fader.FadeAsync(TimeSpan.FromSeconds(o.Wait), TimeSpan.FromSeconds(o.FadeSeconds));
}