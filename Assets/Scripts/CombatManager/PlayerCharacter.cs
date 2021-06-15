using System;
using System.Collections.Generic;

// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : Character
{
    static readonly string[] names = new string[] {
        "Steve",
        "Bob",
        "Joe",
        "Eric"
    };

    private readonly int playerIdentifier;
    public PlayerCharacter(int playerIdentifier) :
        base(names[playerIdentifier],
        null, // can't use this to create a weakref
        null,
        0)
    {
        this.playerIdentifier = playerIdentifier;
        base.combatAttributes = new PlayerAttributes(playerIdentifier, new WeakReference<ICharacter>(this));
        base.healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), 100);
        characterActions.Add(new AttackRandomAction("Sprites/Slime", "Attack"));
        SingletonProvider.MainEventHandler.Subscribe(EventType.PlayerSelectedActionTarget, e =>
        {
            var selAction = e as PlayerSelectedActionTargetEvent;
            characterActions.ForEach(action =>
            {
                if (action == selAction.Action)
                {
                    if (!CombatManager.Instance.InCombat()) throw new Exception("We can't select event if we're not in combat");
                    action.StartChargingAction(this, CombatManager.Instance.CombatReader);
                }
            });
        });
    }

    public override void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        IncrementActions(combat);
    }
}