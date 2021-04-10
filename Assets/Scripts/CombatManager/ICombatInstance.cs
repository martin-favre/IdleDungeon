using System;
using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient, ITimeProvider timeProvider);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    public ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient, ITimeProvider timeProvider)
    {
        return new CombatInstance(playerChars, new EnemyFactory(), evRecipient, timeProvider);
    }
}

public interface ICombatInstance : IDisposable
{
    bool IsDone();
    void Update();

    ICombatReader CombatReader {get;}
}
