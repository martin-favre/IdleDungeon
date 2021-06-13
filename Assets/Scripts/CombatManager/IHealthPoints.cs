public interface IHealthPoints
{
    double MaxHp { get; }
    double CurrentHp { get; }
    void Damage(double damage);
    void Heal(double healing);
    bool IsDead();
}

public interface IHpEventReceiver
{
    void NotifyCurrentHpChanged(double hpDelta);
    void NotifyMaxHpChanged(double hpDelta);
}

public class HealthPoints : IHealthPoints
{
    private double currentHp;
    private double maxHp;
    private readonly IHpEventReceiver hpEventReceiver;

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
            hpEventReceiver.NotifyMaxHpChanged(hpDelta);
        }
    }

    public double CurrentHp => currentHp;

    public HealthPoints(IHpEventReceiver hpEventReceiver, double maxHp)
    {
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.hpEventReceiver = hpEventReceiver;
    }

    public void Damage(double damage)
    {
        var oldCurrentHp = currentHp;
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
        if (currentHp != oldCurrentHp)
        {
            hpEventReceiver.NotifyCurrentHpChanged(currentHp - oldCurrentHp);
        }
    }

    public void Heal(double healing)
    {
        var oldCurrentHp = currentHp;
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
        if (currentHp != oldCurrentHp)
        {
            hpEventReceiver.NotifyCurrentHpChanged(currentHp - oldCurrentHp);
        }

    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }
}