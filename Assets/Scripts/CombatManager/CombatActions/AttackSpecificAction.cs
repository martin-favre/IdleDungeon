
using System;
using UnityEngine;

public class AttackSpecificAction : BaseCharacterAction
{

    public AttackSpecificAction(string icon, string name) : base(icon, name)
    {
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (Target != null && !Target.IsDead())
        {
            Target.BeAttacked(user.Attributes.Attack);
            CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, Target, user, this));
        }
    }
}
