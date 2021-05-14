using System;
using System.Collections.Generic;

public interface IPlayerCharacterUpdateEvent
{

}

public interface IPlayerCharacters : IObservable<IPlayerCharacterUpdateEvent>
{
    PlayerCharacter[] GetAllPlayersChars();
}


/*
    Responsible for creating/loading/storing and distributing the PlayerCharacters
*/
public class PlayerCharacters : IPlayerCharacters, IEventRecipient<IPlayerCharacterUpdateEvent>
{
    static PlayerCharacters instance;
    public static IPlayerCharacters Instance { get => instance; }
    List<PlayerCharacter> playerChars = new List<PlayerCharacter>();
    List<IObserver<IPlayerCharacterUpdateEvent>> observers = new List<IObserver<IPlayerCharacterUpdateEvent>>();

    static PlayerCharacters()
    {
        instance = new PlayerCharacters(SystemRandom.Instance, UpgradeManager.Instance);
    }

    public PlayerCharacters(IRandomProvider randomProvider, IUpgradeManager upgradeManager)
    {
        playerChars.Add(new PlayerCharacter(randomProvider, this, upgradeManager, 0));
    }

    public PlayerCharacter[] GetAllPlayersChars()
    {
        return playerChars.ToArray();
    }

    public void RecieveEvent(IPlayerCharacterUpdateEvent ev)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(ev);
        }
    }

    public IDisposable Subscribe(IObserver<IPlayerCharacterUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<IPlayerCharacterUpdateEvent>(observers, observer);
    }
}