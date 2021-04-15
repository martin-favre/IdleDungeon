public class PlayerAttributes : ICombatAttributes
{
    private readonly IPersistentDataStorage storage;

    public int Hp { get => hp; }

    public int Attack { get => attack; }

    public int Speed => 100;

    private int attack;

    private int hp = 100;

    SimpleObserver<Upgrade> attackinessObserver;

    public PlayerAttributes(IPersistentDataStorage storage)
    {
        this.storage = storage;
        attack = 50 + UpgradeManager.Instance.Attackiness.Level * 50;
        attackinessObserver = new SimpleObserver<Upgrade>(UpgradeManager.Instance.Attackiness, (u) => SetAttack(u.Level));
    }

    void SetAttack(int level)
    {
        attack = 50 + UpgradeManager.Instance.Attackiness.Level * 50;
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0) hp = 0;
    }

    public bool IsDead()
    {
        return false; // Temporary during development to avoid dying all the time
    }
}