using System;
using System.Collections.Generic;
using Logging;
using PubSubSystem;

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

    private static LilLogger logger = new LilLogger(typeof(PlayerCharacter).ToString());
    public PlayerCharacter(int playerIdentifier) :
        base(names[playerIdentifier],
        null, // can't use this to create a weakref
        null,
        0)
    {
        this.playerIdentifier = playerIdentifier;
        base.combatAttributes = new PlayerAttributes(playerIdentifier, new WeakReference<ICharacter>(this));
        base.healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), 100);
        characterActions.Add(new AttackSpecificAction("Sprites/gray_03", "Attack"));
        SingletonProvider.MainEventHandler.Subscribe(EventType.PlayerSelectedActionTarget, OnPlayerSelectedActionTarget);
    }

    private void OnPlayerSelectedActionTarget(IEvent e)
    {
        var selAction = e as PlayerSelectedActionTargetEvent;
        if (selAction.User != this) return;
        characterActions.ForEach(action =>
        {
            if (action == selAction.Action)
            {
                if (action is IHasTarget targetable)
                {
                    targetable.Target = selAction.Target;
                    if (!CombatManager.Instance.InCombat())
                    {
                        logger.Log("We can't select an action if we're not in combat", LogLevel.Error);
                        return;
                    }
                    action.StartChargingAction(this, CombatManager.Instance.CombatReader);
                }
                else
                {
                    logger.Log("We can't target with an action that isn't targetable", LogLevel.Error);
                }
            }
        });

    }

    public override void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        IncrementActions(combat);
    }
}