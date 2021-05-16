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
        instance = new PlayerRoster(SystemRandom.Instance, UpgradeManager.Instance);
    }

    public PlayerRoster(IRandomProvider randomProvider, IUpgradeManager upgradeManager)
    {
        playerChars = new List<PlayerCharacter>() {
            new PlayerCharacter(randomProvider, upgradeManager, 0),
            new PlayerCharacter(randomProvider, upgradeManager, 1),
            new PlayerCharacter(randomProvider, upgradeManager, 2),
            new PlayerCharacter(randomProvider, upgradeManager, 3)
        };

    }

    public PlayerCharacter[] GetAllPlayersChars()
    {
        return playerChars.ToArray();
    }
}