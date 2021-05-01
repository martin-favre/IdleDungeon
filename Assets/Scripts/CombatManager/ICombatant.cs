using System;
using System.Collections.Generic;

public interface ICombatant
{
    void PerformAction(List<ICombatant> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient);
    void BeAttacked(double attackStat);
    bool IsDead();
    ICombatAttributes Attributes { get; }
    ITurnProgress TurnProgress { get; }

    Guid UniqueId { get; }
}
