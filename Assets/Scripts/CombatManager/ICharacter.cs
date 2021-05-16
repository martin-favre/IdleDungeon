using System;
using System.Collections.Generic;

public interface ICharacter
{
    void PerformAction(List<ICharacter> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient);
    void BeAttacked(double attackStat);
    bool IsDead();
    ICombatAttributes Attributes { get; }

    double ExperienceWorth { get; }

    ITurnProgress TurnProgress { get; }

    Guid UniqueId { get; }
}
