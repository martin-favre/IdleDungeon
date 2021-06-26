using System;
using System.Collections.Generic;

public interface ICharacter
{
    void PerformAction(List<ICharacter> enemies, ICombatReader combat);
    void BeAttacked(ICharacter assailant, AttackConfig attack);
    bool IsDead();
    void ApplyBuff(IBuff buff);
    bool IsPlayer { get; }
    float EffectiveAttack { get; }
    float EffectiveSpeed { get; }
    IBuff[] Buffs { get; }
    string Name { get; }
    ICombatAttributes Attributes { get; }
    double ExperienceWorth { get; }
    double GoldWorth { get; }
    IGuid UniqueId { get; }
    IHealthPoints HealthPoints { get; }
    ICharacterAction[] CharacterActions { get; }
}
