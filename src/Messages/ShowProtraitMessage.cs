namespace HonkaiCharacterBrowser.Messages;

public class ShowProtraitMessage
{
    public string VideoSource { get; set; }

    public ShowProtraitMessage(string videoSource)
    {
        VideoSource = videoSource;
    }
}
