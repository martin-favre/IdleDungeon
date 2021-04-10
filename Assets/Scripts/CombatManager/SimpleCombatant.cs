using System.Collections.Generic;

class SimpleCombatant : ICombatant
{
    private readonly CombatAttributes attributes = new CombatAttributes();
    private readonly TurnProgress turnProgress = new TurnProgress();

    public CombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public void BeAttacked(int attackStat)
    {
        int dmg = attributes.CalculateDamage(attackStat);
        attributes.Damage(dmg);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void PerformAction(List<ICombatant> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICombatant target = enemies[0];
        target.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, target, this));
    }


}
