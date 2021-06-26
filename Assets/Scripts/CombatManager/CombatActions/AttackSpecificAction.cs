
using System;
using UnityEngine;

public class AttackSpecificAction : BaseCharacterAction
{

    public AttackSpecificAction(string iconPath, float baseActionTime, DamageConfig damageConfig) : base(iconPath, baseActionTime, damageConfig, true, false)
    {
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (Target != null && !Target.IsDead())
        {
            var minDmg = Mathf.RoundToInt(Damage.MinimumDamage * user.Attributes.Attack);
            var maxDmg = Mathf.RoundToInt(Damage.MaximumDamage * user.Attributes.Attack);
            var dmg = SingletonProvider.MainRandomProvider.RandomInt(minDmg, maxDmg);
            Target.BeAttacked(user, new AttackConfig(dmg));
            CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, Target, user, this));
        }
    }
}
