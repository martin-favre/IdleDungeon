public interface ICombatAttributes
{
    int MaxHp { get; }
    int CurrentHp { get; }
    int Attack { get; }
    int Speed { get; }
    void Damage(int damage);
    bool IsDead();
}
