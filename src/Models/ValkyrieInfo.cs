namespace HonkaiCharacterBrowser.Models;

public record ValkyrieInfo(
    string Name,
    string Armor,
    string Birthday,
    string BattleStyle,
    string ProtraitUrl,
    Skill[] Skills
);
