
using System;
using UnityEngine;

public class AttackSpecificAction : BaseCharacterAction
{
    private ICharacter target;

    public AttackSpecificAction(string icon, string name) : base(icon, name)
    {
    }

    public ICharacter Target { get => target; set => target = value; }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (target != null && !target.IsDead())
        {
            target.BeAttacked(user.Attributes.Attack);
            CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, target, user, this));
        }
    }
}
