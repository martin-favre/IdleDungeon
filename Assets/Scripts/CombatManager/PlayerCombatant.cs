using System.Collections.Generic;

class PlayerCombatant : ICombatant
{
    private readonly IRandomProvider random;

    public PlayerCombatant(IRandomProvider random) {
        this.random = random;
    }
    public void PerformAction(List<ICombatant> enemies)
    {
        ICombatant enemy = Helpers.GetRandom<ICombatant>(enemies, random);
    }

    public void BeAttacked(int AttackStat)
    {
         
    }
}
