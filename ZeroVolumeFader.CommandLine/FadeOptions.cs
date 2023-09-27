using CommandLine;

public class FadeOptions
{
    [Option('w', "Wait time", Required = false, HelpText = "Set desired wait time.")]
    public int Wait { get; set; }

    [Option('f', "Fade time", Required = true, HelpText = "Set desired fade time.")]
    public int FadeSeconds { get; set; }
}