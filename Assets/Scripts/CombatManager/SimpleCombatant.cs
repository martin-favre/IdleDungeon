using System;
using System.Collections.Generic;

class SimpleCombatant : ICombatant
{
    private readonly SimpleCombatAttributes attributes = new SimpleCombatAttributes();
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
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
