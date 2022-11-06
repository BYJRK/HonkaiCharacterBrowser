namespace HonkaiCharacterBrowser.Models;

public record Skill(
    string Title,
    string IconUrl,
    string Description,
    string PreviewUrl,
    string VideoUrl
    )
{
    public Skill() : this("", "", "", "", "")
    {
    }
}