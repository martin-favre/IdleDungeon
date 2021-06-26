using System;
using System.Collections.Generic;

/*
    Responsible for creating/loading/storing and distributing the PlayerCharacters
*/
public class PlayerRoster : IPlayerRoster
{
    static PlayerRoster instance;
    public static IPlayerRoster Instance { get => instance; }
    List<PlayerCharacter> playerChars;

    static PlayerRoster()
    {
        instance = new PlayerRoster();
    }

    public PlayerRoster()
    {
        var warrior = new PlayerCharacter(0,
            maxHp: 30,
            new PlayerAttributes(1, 1),
            new[] { new AttackSpecificAction("Sprites/yellow_29", 8, new DamageConfig(8, 12)) }
        );
        var rogue = new PlayerCharacter(1,
                    maxHp: 25,
                    new PlayerAttributes(0.7f, 2),
                    new ICharacterAction[] { new AttackSpecificAction("Sprites/gray_03", 4, new DamageConfig(4, 6)),
                        new DefendAllyAction("Sprites/addon_04", 4)}
                );
        playerChars = new List<PlayerCharacter>() {
            warrior,
            rogue,
            // new PlayerCharacter(2),
            // new PlayerCharacter(3)
        };

    }

    public PlayerCharacter[] GetAllPlayersChars()
    {
        return playerChars.ToArray();
    }
}