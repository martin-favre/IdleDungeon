public interface IHealthPoints {
    double MaxHp { get; }
    double CurrentHp { get; }
    void Damage(double damage);
    void Heal(double healing);
    bool IsDead();
}