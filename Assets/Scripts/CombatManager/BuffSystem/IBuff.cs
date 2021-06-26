using System;
using UnityEngine;

public interface IBuff
{
    Sprite Icon { get; }
    int Stacks { get; }
    bool IsExpired();
    void OnOtherBuffAdded(IBuff other);
    float GetAttack(float attack);
    float GetSpeed(float speed);

    // Forcibly expire the buff
    void Expire();

    // returns resulting damage
    AttackConfig OnCharacterAttacked(ICharacter victim, ICharacter assailant, AttackConfig attack);
}

public abstract class BaseBuff : IBuff
{
    protected int stacks = 1;
    bool forceExpire = false;

    private readonly Sprite icon;

    protected BaseBuff(string iconPath)
    {
        this.icon = SingletonProvider.MainGameobjectLoader.GetPrefab<Sprite>(iconPath);
    }

    public int Stacks { get => stacks; }

    public Sprite Icon => icon;

    public float GetAttack(float attack)
    {
        return attack;
    }

    public float GetSpeed(float speed)
    {
        return speed;
    }

    public virtual bool IsExpired()
    {
        return forceExpire;
    }
    public void Expire()
    {
        forceExpire = true;
    }

    public abstract void OnOtherBuffAdded(IBuff other);

    public virtual AttackConfig OnCharacterAttacked(ICharacter victim, ICharacter assailant, AttackConfig attack) { return attack; }

}

public class DefendBuff : BaseBuff
{
    WeakReference<ICharacter> casterRef;
    SimpleTimer timer;
    bool forceExpire = false;

    public DefendBuff(WeakReference<ICharacter> caster) : base("Sprites/yellow_34")
    {
        this.casterRef = caster;
        timer = new SimpleTimer(10);
    }

    public override bool IsExpired()
    {
        if (base.IsExpired()) return true;
        if (timer.IsDone()) return true;
        ICharacter caster;
        var success = casterRef.TryGetTarget(out caster);
        if (!success) return true;
        return false;
    }

    public override void OnOtherBuffAdded(IBuff other)
    {
        if (other is DefendBuff)
        {
            forceExpire = true;
        }
    }

    public override AttackConfig OnCharacterAttacked(ICharacter victim, ICharacter assailant, AttackConfig attack)
    {
        if (IsExpired() || attack.IsReflected) return attack;
        ICharacter caster;
        var success = casterRef.TryGetTarget(out caster);
        if (success)
        {
            caster.BeAttacked(victim, new AttackConfig(attack.Damage, true));
            return new AttackConfig();
        }
        return attack;
    }
}

public class ThornsBuff : BaseBuff
{
    SimpleTimer timer;

    public ThornsBuff() : base("Sprites/yellow_34")
    {
        timer = new SimpleTimer(10);
    }

    public override bool IsExpired()
    {
        if (base.IsExpired()) return true;
        if (timer.IsDone()) return true;
        return false;
    }

    public override AttackConfig OnCharacterAttacked(ICharacter victim, ICharacter assailant, AttackConfig attack)
    {
        if (IsExpired() || attack.IsReflected) return attack;
        assailant.BeAttacked(victim, new AttackConfig(5 * stacks, true));
        return attack;
    }

    public override void OnOtherBuffAdded(IBuff other)
    {
        timer = new SimpleTimer(10);
        stacks += 1;
        other.Expire();
    }
}


