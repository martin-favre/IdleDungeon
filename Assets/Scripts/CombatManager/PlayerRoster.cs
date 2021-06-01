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
        playerChars = new List<PlayerCharacter>() {
            new PlayerCharacter(0),
            new PlayerCharacter(1),
            new PlayerCharacter(2),
            new PlayerCharacter(3)
        };

    }

    public PlayerCharacter[] GetAllPlayersChars()
    {
        return playerChars.ToArray();
    }
}