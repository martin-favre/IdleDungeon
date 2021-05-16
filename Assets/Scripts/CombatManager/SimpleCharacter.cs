using System;
using System.Collections.Generic;

class SimpleCombatant : ICharacter
{
    private readonly SimpleCombatAttributes attributes = new SimpleCombatAttributes();
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public double ExperienceWorth => 0;

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void PerformAction(List<ICharacter> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICharacter target = enemies[0];
        target.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, target, this));
    }


}
