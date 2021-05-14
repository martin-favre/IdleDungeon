using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsDisplayer : MonoBehaviour
{
    Dictionary<Guid, SimpleValueDisplayer.ValueHook> displayers = new Dictionary<Guid, SimpleValueDisplayer.ValueHook>();
    SimpleObserver<ICombatUpdateEvent> observer;

    void Start()
    {
        observer = new SimpleObserver<ICombatUpdateEvent>(CombatManager.Instance, (e) =>
        {
            if (e is EnteredCombatEvent)
            {
                foreach (var enemy in e.Combat.GetEnemies())
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
            else
            {
                UpdateText(e.Combat);
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