using System.Collections.Generic;

class SimpleCombatant : ICombatant
{
    private readonly CombatAttributes attributes = new CombatAttributes();

    public CombatAttributes Attributes => attributes;

    public void BeAttacked(int attackStat)
    {
        int dmg = attributes.CalculateDamage(attackStat);
        attributes.Damage(dmg);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void PerformAction(List<ICombatant> enemies)
    {
        if (enemies.Count == 0) return;
        ICombatant target = enemies[0];
        target.BeAttacked(attributes.Attack);
    }

    
}
