using System;
using System.Collections.Generic;
using PubSubSystem;
using UnityEngine;

public class EnemyStatsDisplayer : MonoBehaviour
{
    Dictionary<IGuid, SimpleValueDisplayer.ValueHook> displayers = new Dictionary<IGuid, SimpleValueDisplayer.ValueHook>();
    Subscription<int> subscription;

    void Start()
    {
        subscription = CombatEventPublisher.Instance.Subscribe((e) =>
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
            else if(e is ICombatUpdateEvent cue)
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
                displayers[item.UniqueId].UpdateValue("Enemy HP: " + Mathf.RoundToInt((float)item.Attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)item.Attributes.MaxHp));
            }
        }
    }
}