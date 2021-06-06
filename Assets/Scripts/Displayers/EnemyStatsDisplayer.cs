using System;
using System.Collections.Generic;
using PubSubSystem;
using UnityEngine;

public class EnemyStatsDisplayer : MonoBehaviour
{
    Dictionary<IGuid, SimpleValueDisplayer.ValueHook> displayers = new Dictionary<IGuid, SimpleValueDisplayer.ValueHook>();
    Subscription<EventType> subscription;

    void Start()
    {
        subscription = MainEventHandler.Instance.Subscribe(new[] { EventType.CombatStarted, EventType.CombatEnded, EventType.CombatAction, EventType.CombatantDied }, (e) =>
         {
             if (e is EnteredCombatEvent ent)
             {
                 foreach (var enemy in ent.Combat.GetEnemies())
                 {
                     displayers[enemy.UniqueId] = SimpleValueDisplayer.Instance.RegisterValue();
                 }
             }

             if (e is ExitedCombatEvent)
             {
                 foreach (var item in displayers)
                 {
                     item.Value.Dispose();
                 }
                 displayers.Clear();
             }
             else if (e is ICombatUpdateEvent cue)
             {
                 UpdateText(cue.Combat);
             }
         });
    }

    private void UpdateText(ICombatReader combat)
    {
        if (combat != null)
        {
            foreach (var item in combat.GetEnemies())
            {
                if (item.Attributes is IHealthPoints hp)
                {
                    displayers[item.UniqueId].UpdateValue("Enemy HP: " + Mathf.RoundToInt((float)hp.CurrentHp) + "/" + Mathf.RoundToInt((float)hp.MaxHp));
                }
            }
        }
    }
}