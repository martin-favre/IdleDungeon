using System.Collections;
using System.Collections.Generic;
using PubSubSystem;
using UnityEngine;

public class CombatResultSpawnerComponent : MonoBehaviour
{
    [SerializeField]
    CombatResultScreenComponent combatResultScreen;
    Subscription<EventType> subscription;

    CombatResult result;

    void Awake()
    {
        subscription = SingletonProvider.MainEventPublisher.Subscribe(EventType.CombatEnded, OnEvent);
        combatResultScreen.gameObject.SetActive(false);
    }

    void OnEvent(IEvent e)
    {
        if (e is CombatEndedEvent combatEnded)
        {
            combatResultScreen.gameObject.SetActive(true);
            combatResultScreen.SetCombatResult(combatEnded.Result);
            result = combatEnded.Result;
        }
    }

    public void OnContinuePressed()
    {
        SingletonProvider.MainEventPublisher.Publish(EventType.CombatResultsClosed, new CombatResultsClosedEvent(result));
        SingletonProvider.MainPlayerWallet.AddGold(result.GainedGold);
        result = null;
        combatResultScreen.gameObject.SetActive(false);
    }
}
