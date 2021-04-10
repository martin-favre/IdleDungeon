using System.Collections.Generic;

public interface ICombatant
{
    void PerformAction(List<ICombatant> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient);
    void BeAttacked(int attackStat);
    bool IsDead();
    CombatAttributes Attributes { get; }
    ITurnProgress TurnProgress { get; }
}
