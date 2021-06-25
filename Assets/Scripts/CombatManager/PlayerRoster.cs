using System;
using System.Collections.Generic;

public interface IPlayerRoster
{
    PlayerCharacter[] GetAllPlayersChars();
}


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
            30,
            new PlayerAttributes(1, 1),
            new[] { new AttackSpecificAction("Sprites/yellow_29", "Attack", 8, 8, 12) }
        );
        var rogue = new PlayerCharacter(1,
                    25,
                    new PlayerAttributes(0.7f, 2),
                    new[] { new AttackSpecificAction("Sprites/gray_03", "Attack", 4, 4, 6) }
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