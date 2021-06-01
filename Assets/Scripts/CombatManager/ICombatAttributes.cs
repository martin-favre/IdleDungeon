using System;

public interface ICombatAttributes: IDisposable
{
    double MaxHp { get; }
    double CurrentHp { get; }
    double Attack { get; }
    double Speed { get; }
    void Damage(double damage);
    void Heal(double healing);
    bool IsDead();
}
