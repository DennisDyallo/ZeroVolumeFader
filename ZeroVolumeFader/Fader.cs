using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ZeroVolumeFader;
public class Fader
{
    private readonly ILogger<Fader> _logger;
    private readonly ISystemAudio _systemAudio;
    private const int OffsetTickWaitMs = 200;
    public TimeSpan Elapsed => _sw.Elapsed;
    private readonly Stopwatch _sw = new ();

    public Fader(ISystemAudio systemAudio, ILogger<Fader> logger)
    {
        _logger = logger;
        _systemAudio = systemAudio;
    }

    public async Task FadeAsync(
        TimeSpan waitTime = default, 
        TimeSpan fadeTime = default,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Starting with wait time {waitTime.TotalSeconds} and fade time over {fadeTime}");
        _sw.Reset();
        _sw.Start();
        await Task.Delay(waitTime, cancellationToken);

        var volumeDistanceToFade = _systemAudio.Level;
        var fadeRate = OffsetTickWaitMs * (volumeDistanceToFade / fadeTime.TotalMilliseconds);

        while (_systemAudio.Level > fadeRate)
        {
            _logger.LogDebug($"Reducing volume by {fadeRate}, current level is {_systemAudio.Level}");
            _systemAudio.Level -= (float)fadeRate;
            await Task.Delay(OffsetTickWaitMs, cancellationToken);
        }
        
        await Task.Delay(OffsetTickWaitMs, cancellationToken);
        _systemAudio.Level = 0;
        _sw.Stop();
        _logger.LogInformation($"Finished fade after {_sw.Elapsed.TotalSeconds}");
    }
}