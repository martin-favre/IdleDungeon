
using System;
using UnityEngine;

public class AttackSpecificAction : BaseCharacterAction
{
    private readonly int minDamage;
    private readonly int maxDamage;

    public AttackSpecificAction(string icon, string name, float baseActionTimeS, int minDamage, int maxDamage) : base(icon, name, baseActionTimeS)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (Target != null && !Target.IsDead())
        {
            var minDmg = Mathf.RoundToInt(minDamage * user.Attributes.Attack);
            var maxDmg = Mathf.RoundToInt(maxDamage * user.Attributes.Attack);
            Target.BeAttacked(SingletonProvider.MainRandomProvider.RandomInt(minDmg, maxDmg));
            CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, Target, user, this));
        }
    }
}
