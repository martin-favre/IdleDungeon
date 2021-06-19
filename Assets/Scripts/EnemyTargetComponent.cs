using System;
using System.Collections;
using System.Collections.Generic;
using PubSubSystem;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyTargetComponent : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Subscription<EventType> clickSubscription;
    private ICharacter character;
    private ICharacterAction awaitedAction;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        clickSubscription = SingletonProvider.MainEventHandler.Subscribe(new[] { EventType.PlayerSelectedActionTarget, EventType.CombatAction }, OnPlayerClickedEnemy);
        spriteRenderer.enabled = false;
    }

    internal void SetCharacter(ICharacter character)
    {
        this.character = character;
    }

    void OnPlayerClickedEnemy(IEvent e)
    {
        if (e is PlayerSelectedActionTargetEvent clk)
        {
            if (clk.Target == character)
            {
                awaitedAction = clk.Action;
                spriteRenderer.enabled = true;
            }
        }
        else if (e is CombatActionEvent act)
        {
            if (act.Target == character && act.Action == awaitedAction)
            {
                spriteRenderer.enabled = false;
                awaitedAction = null;
            }
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
