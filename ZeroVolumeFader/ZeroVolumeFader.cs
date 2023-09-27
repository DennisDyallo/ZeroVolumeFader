using System.Diagnostics;

namespace ZeroVolumeFader;

public class ZeroVolumeFader
{
    private readonly ISystemAudio _systemAudio;
    private const int OffsetTickWaitMs = 200;

    public ZeroVolumeFader(ISystemAudioStore systemAudioStore)
    {
        _systemAudio = systemAudioStore.Get();
    }
    public async Task Fade(TimeSpan waitTime = default, TimeSpan fadeTime = default, CancellationToken cancellationToken = default)
    {
        await Task.Delay(waitTime, cancellationToken);

        var volumeDistanceToFade = _systemAudio.Level;
        var fadeRate = OffsetTickWaitMs * (volumeDistanceToFade / fadeTime.TotalMilliseconds);

        while (_systemAudio.Level > fadeRate)
        {
            _systemAudio.Level = (float)(_systemAudio.Level - fadeRate);
            await Task.Delay(OffsetTickWaitMs, cancellationToken);
        }
        _systemAudio.Level = 0;
        await Task.Delay(OffsetTickWaitMs, cancellationToken);
    }
}

public interface ISystemAudioStore
{
    public ISystemAudio Get();
}
public class SystemAudioStore: ISystemAudioStore
{
    private static readonly SystemAudio SystemAudio = new ();
    public ISystemAudio Get() => SystemAudio;
}
public interface ISystemAudio
{
    public float Level { get; set; }
}

public class SystemAudio : ISystemAudio
{
    public float Level { get; set; } = 100;
}