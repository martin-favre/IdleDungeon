using System.Collections.Generic;


class PlayerCombatant : ICombatant
{
    private readonly IRandomProvider random;
    private readonly CombatAttributes attributes = new CombatAttributes();

    public CombatAttributes Attributes => attributes;

    public PlayerCombatant(IRandomProvider random)
    {
        this.random = random;
    }

    public void PerformAction(List<ICombatant> enemies)
    {
        if (enemies.Count == 0) return;
        ICombatant enemy = Helpers.GetRandom<ICombatant>(enemies, random);
        enemy.BeAttacked(attributes.Attack);
    }

    public void BeAttacked(int attackStat)
    {
        // int dmg = attributes.CalculateDamage(attackStat);
        // attributes.Damage(dmg);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }
}
