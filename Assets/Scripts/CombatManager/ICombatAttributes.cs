public interface ICombatAttributes
{
    int Hp { get; }
    int Attack { get; }
    int Speed { get; }
    void Damage(int damage);
    bool IsDead();
}
