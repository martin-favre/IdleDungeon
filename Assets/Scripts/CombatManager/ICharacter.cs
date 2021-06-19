using System;
using System.Collections.Generic;

public interface ICharacter
{
    void PerformAction(List<ICharacter> enemies, ICombatReader combat);
    void BeAttacked(double attackStat);
    bool IsDead();
    string Name { get; }
    ICombatAttributes Attributes { get; }
    double ExperienceWorth { get; }
    double GoldWorth { get; }
    IGuid UniqueId { get; }

    IHealthPoints HealthPoints { get; }

    ICharacterAction[] CharacterActions { get; }
}
