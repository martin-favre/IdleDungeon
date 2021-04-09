using System;
using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    public ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        return new CombatInstance(playerChars, new EnemyFactory(), evRecipient);
    }
}

public interface ICombatInstance : IDisposable
{
    bool IsDone();
    void Update();
}
