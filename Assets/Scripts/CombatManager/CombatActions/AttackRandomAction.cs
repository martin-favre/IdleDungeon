
using System;
using UnityEngine;

public class AttackRandomAction : BaseCharacterAction
{
    private readonly int minDamage;
    private readonly int maxDamage;

    public AttackRandomAction(string icon, string name, float baseActionSpeedS, int minDamage, int maxDamage) : base(icon, name, baseActionSpeedS)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        var enemies = user is PlayerCharacter ? combat.GetEnemies() : combat.GetPlayers();
        if (enemies.Length == 0) return;
        ICharacter target = enemies[SingletonProvider.MainRandomProvider.RandomInt(0, enemies.Length)];
        var minDmg = Mathf.RoundToInt(minDamage*user.Attributes.Attack);
        var maxDmg = Mathf.RoundToInt(maxDamage*user.Attributes.Attack);

        target.BeAttacked(SingletonProvider.MainRandomProvider.RandomInt(minDmg, maxDmg));
        CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, target, user, this));
    }
}
