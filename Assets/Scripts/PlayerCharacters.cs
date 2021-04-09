using System;
using System.Collections.Generic;

public class PlayerCharacterUpdateEvent
{
    private readonly PlayerCharacter character;

    public PlayerCharacterUpdateEvent(PlayerCharacter character)
    {
        this.character = character;
    }

    public PlayerCharacter Character => character;
}

/*
    Responsible for creating/loading/storing and distributing the PlayerCharacters
*/
public class PlayerCharacters : IObservable<PlayerCharacterUpdateEvent>, IEventRecipient<PlayerCharacterUpdateEvent>
{
    static PlayerCharacters instance;

    List<PlayerCharacter> playerChars = new List<PlayerCharacter>();

    List<IObserver<PlayerCharacterUpdateEvent>> observers = new List<IObserver<PlayerCharacterUpdateEvent>>();

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

    public void RecieveEvent(PlayerCharacterUpdateEvent ev)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(ev);
        }
    }

    public IDisposable Subscribe(IObserver<PlayerCharacterUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<PlayerCharacterUpdateEvent>(observers, observer);
    }
}