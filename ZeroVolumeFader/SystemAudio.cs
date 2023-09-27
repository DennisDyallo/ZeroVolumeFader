using NAudio.CoreAudioApi;

namespace ZeroVolumeFader;

public interface ISystemAudio
{
    public float Level { get; set; }
}

public class SystemAudio : ISystemAudio
{
    private static MMDevice _mmDevice = null!;

    public SystemAudio()
    {
        using var mmDeviceEnumerator = new MMDeviceEnumerator();
        _mmDevice = mmDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    public float Level
    {
        get => _mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
        set => _mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value;
    }
}