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
        null)
    {
        this.playerIdentifier = playerIdentifier;
        base.combatAttributes = new PlayerAttributes(playerIdentifier, new WeakReference<ICharacter>(this));
        base.healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), 100);
        possibleCharacterActions.Add(new AttackSpecificAction("Sprites/gray_03", "Attack"));
        SingletonProvider.MainEventPublisher.Subscribe(EventType.PlayerSelectedActionTarget, OnPlayerSelectedActionTarget);
    }

    private void OnPlayerSelectedActionTarget(IEvent e)
    {
        var selAction = e as PlayerSelectedActionTargetEvent;
        if (selAction.User != this) return;
        StartChargingAction(selAction.Action, selAction.Target, SingletonProvider.MainCombatManager.CombatReader);
    }

    public override void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        IncrementActions(combat);
    }
}