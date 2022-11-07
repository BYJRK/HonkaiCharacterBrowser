namespace HonkaiCharacterBrowser.Messages;

public class PlayVideoMessage
{
    public string VideoSource { get; set; }

    public PlayVideoMessage(string videoSource)
    {
        VideoSource = videoSource;
    }
}
