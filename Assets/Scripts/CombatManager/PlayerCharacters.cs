using System;
using System.Collections.Generic;

public interface IPlayerCharacterUpdateEvent
{

}


/*
    Responsible for creating/loading/storing and distributing the PlayerCharacters
*/
public class PlayerCharacters : IObservable<IPlayerCharacterUpdateEvent>, IEventRecipient<IPlayerCharacterUpdateEvent>
{
    static PlayerCharacters instance;

    List<PlayerCharacter> playerChars = new List<PlayerCharacter>();

    List<IObserver<IPlayerCharacterUpdateEvent>> observers = new List<IObserver<IPlayerCharacterUpdateEvent>>();

    static PlayerCharacters()
    {
        instance = new PlayerCharacters(SystemRandom.Instance);
    }

    public PlayerCharacters(IRandomProvider randomProvider)
    {
        playerChars.Add(new PlayerCharacter(randomProvider, this));
    }

    public static PlayerCharacters Instance { get => instance; }

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