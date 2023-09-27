using System.Diagnostics;

namespace ZeroVolumeFader.Tests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var systemAudioStore = new SystemAudioStore();
        var zeroVolumeFader = new ZeroVolumeFader(systemAudioStore);
        var sw = new Stopwatch();
        sw.Start();
        await zeroVolumeFader.Fade(fadeTime: TimeSpan.FromSeconds(2));
        Assert.Equal(0, systemAudioStore.Get().Level);
    }
}