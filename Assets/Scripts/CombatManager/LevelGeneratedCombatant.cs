using System;
using System.Collections.Generic;

class LevelGeneratedCombatant : ICombatant
{
    private readonly LevelGeneratedCombatAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public LevelGeneratedCombatant(int currentLevel) {
        attributes = new LevelGeneratedCombatAttributes(currentLevel);
    }

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
