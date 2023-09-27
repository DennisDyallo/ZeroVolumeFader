using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;

namespace ZeroVolumeFader.Tests;

public class FaderTests
{
    [Fact]
    public async void CanFadeMockAudioWithinTime()
    {
        var systemAudioMock = new SystemAudioMock();
        var zeroVolumeFader = new Fader(systemAudioMock, new NullLogger<Fader>());
        
        await zeroVolumeFader.FadeAsync(fadeTime: TimeSpan.FromSeconds(2));
        
        Assert.Equal(0, systemAudioMock.Level);
        await Task.Delay(TimeSpan.FromMilliseconds(500));
        Assert.InRange(zeroVolumeFader.Elapsed.TotalSeconds, 2, 2.1);
    }
    
    [Fact]
    public async void CanFadeRealAudioWithinTime()
    {
        var systemAudio = new SystemAudio { Level = 1 };
        var zeroVolumeFader = new Fader(systemAudio, new NullLogger<Fader>());
        
        await zeroVolumeFader.FadeAsync(fadeTime: TimeSpan.FromSeconds(2));
        
        Assert.Equal(0, systemAudio.Level);
        await Task.Delay(TimeSpan.FromMilliseconds(500));
        Assert.InRange(zeroVolumeFader.Elapsed.TotalSeconds, 2, 2.1);
        systemAudio.Level = 1;
    }

    public class SystemAudioMock : ISystemAudio
    {
        public float Level { get; set; } = 1;
    }
}