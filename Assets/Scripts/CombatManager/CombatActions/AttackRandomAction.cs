
using System;
using UnityEngine;

public class AttackRandomAction : BaseCharacterAction
{

    public AttackRandomAction(string icon, string name) : base(icon, name){}

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        var enemies = user is PlayerCharacter ? combat.GetEnemies() : combat.GetPlayers();
        if (enemies.Length == 0) return;
        ICharacter target = enemies[SingletonProvider.MainRandomProvider.RandomInt(0, enemies.Length)];
        target.BeAttacked(user.Attributes.Attack);
        CentralEventHandler.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, target, user));
    }
}
