using CommandLine;

public class FadeOptions
{
    [Option('w', "Wait", Required = false, HelpText = "Set desired wait time in seconds.")]
    public int Wait { get; set; }

    [Option('f', "Fade", Required = true, HelpText = "Set desired fade time in seconds.")]
    public int FadeSeconds { get; set; }
}