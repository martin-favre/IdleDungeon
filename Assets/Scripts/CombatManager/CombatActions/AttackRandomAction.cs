
using System;
using Logging;
using UnityEngine;

public class AttackRandomAction : BaseCharacterAction
{
    private static readonly LilLogger logger = new LilLogger(typeof(AttackRandomAction).ToString());
    public AttackRandomAction(string iconPath, float baseActionTime, DamageConfig damage) : base(iconPath, baseActionTime, damage, true, false)
    {
    }
    public override void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat)
    {
        var enemies = user.IsPlayer ? combat.GetEnemies() : combat.GetPlayers();
        if (enemies.Length == 0)
        {
            logger.Log("Unable to find a target to attack", LogLevel.Warning);
        }
        ICharacter actualTarget = enemies[SingletonProvider.MainRandomProvider.RandomInt(0, enemies.Length)];
        base.StartChargingAction(user, actualTarget, combat);
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (Target != null && !Target.IsDead())
        {
            var minDmg = Mathf.RoundToInt(Damage.MinimumDamage * user.EffectiveAttack);
            var maxDmg = Mathf.RoundToInt(Damage.MaximumDamage * user.EffectiveSpeed);
            var dmg = SingletonProvider.MainRandomProvider.RandomInt(minDmg, maxDmg);
            Target.BeAttacked(user, new AttackConfig(dmg));
            CentralEventPublisher.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, Target, user, this));
        }
    }
}
