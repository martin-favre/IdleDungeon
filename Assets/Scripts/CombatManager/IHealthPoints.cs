using System;

public interface IHealthPoints
{
    double MaxHp { get; }
    double CurrentHp { get; }
    void Damage(double damage);
    void Heal(double healing);
    bool IsDead();
}


public class HealthPoints : IHealthPoints
{
    private double currentHp;
    private readonly WeakReference<ICharacter> character;
    private double maxHp;
    public double MaxHp
    {
        get => maxHp;
        set
        {
            // On maxhp changed retain % currentHp
            if (maxHp == value) return;
            var prevHpFactor = currentHp / maxHp;
            var hpDelta = value - maxHp;
            maxHp = value;
            currentHp = maxHp * prevHpFactor;
            ICharacter chr;
            if (character.TryGetTarget(out chr))
            {
                MainEventHandler.Instance.Publish(EventType.CharacterMaxHpChanged, new MaxHpChanged(hpDelta, chr));
            }

        }
    }

    public double CurrentHp => currentHp;

    public HealthPoints(WeakReference<ICharacter> character, double maxHp)
    {
        this.character = character;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
    }

    public void Damage(double damage)
    {
        var oldCurrentHp = currentHp;
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
        if (currentHp != oldCurrentHp)
        {
            ICharacter chr;
            if (character.TryGetTarget(out chr))
            {
                MainEventHandler.Instance.Publish(EventType.CharacterCurrentHpChanged, new CurrentHpChanged(currentHp - oldCurrentHp, chr));
            }
        }
    }

    public void Heal(double healing)
    {
        var oldCurrentHp = currentHp;
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
        if (currentHp != oldCurrentHp)
        {
            ICharacter chr;
            if (character.TryGetTarget(out chr))
            {
                MainEventHandler.Instance.Publish(EventType.CharacterCurrentHpChanged, new CurrentHpChanged(currentHp - oldCurrentHp, chr));
            }
        }

    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }
}