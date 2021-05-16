using System;
using System.Collections.Generic;

public interface IPlayerRosterUpdateEvent
{

}

public interface IPlayerRoster : IObservable<IPlayerRosterUpdateEvent>
{
    PlayerCharacter[] GetAllPlayersChars();
}


/*
    Responsible for creating/loading/storing and distributing the PlayerCharacters
*/
public class PlayerRoster : IPlayerRoster, IEventRecipient<IPlayerRosterUpdateEvent>
{
    static PlayerRoster instance;
    public static IPlayerRoster Instance { get => instance; }
    List<PlayerCharacter> playerChars;
    List<IObserver<IPlayerRosterUpdateEvent>> observers = new List<IObserver<IPlayerRosterUpdateEvent>>();

    static PlayerRoster()
    {
        instance = new PlayerRoster(SystemRandom.Instance, UpgradeManager.Instance);
    }

    public PlayerRoster(IRandomProvider randomProvider, IUpgradeManager upgradeManager)
    {
        playerChars = new List<PlayerCharacter>() {
            new PlayerCharacter(randomProvider, this, upgradeManager, 0),
            new PlayerCharacter(randomProvider, this, upgradeManager, 1),
            new PlayerCharacter(randomProvider, this, upgradeManager, 2),
            new PlayerCharacter(randomProvider, this, upgradeManager, 3)
        };

    }

    public PlayerCharacter[] GetAllPlayersChars()
    {
        return playerChars.ToArray();
    }

    public void RecieveEvent(IPlayerRosterUpdateEvent ev)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(ev);
        }
    }

    public IDisposable Subscribe(IObserver<IPlayerRosterUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<IPlayerRosterUpdateEvent>(observers, observer);
    }
}