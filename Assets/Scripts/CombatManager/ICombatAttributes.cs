public interface ICombatAttributes
{
    double MaxHp { get; }
    double CurrentHp { get; }
    double Attack { get; }
    double Speed { get; }
    void Damage(double damage);
    bool IsDead();
}