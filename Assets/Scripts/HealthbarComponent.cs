using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PubSubSystem;

public class HealthbarComponent : MonoBehaviour
{

    [SerializeField]
    private UIBarComponent hpBar;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private PoppingTextSpawnerComponent textSpawner;

    Subscription<EventType> subscription;

    IGuid targetGuid;

    public void SetAttributes(ICharacter character)
    {
        targetGuid = character.UniqueId;
        if (subscription != null) subscription.Dispose();
        subscription = CentralEventPublisher.Instance.Subscribe(new[] { EventType.CharacterAttributeChanged, EventType.CharacterCurrentHpChanged }, HandleEvent);
        if (character.HealthPoints != null)
        {
            UpdateBarFill(character);
            UpdateText(character);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleEvent(IEvent ev)
    {
        if (ev is CharacterUpdateEvent chr && chr.Character.UniqueId.Equals(targetGuid))
        {
            UpdateBarFill(chr.Character);
            UpdateText(chr.Character);
            if (ev is CurrentHpChanged hpChanged)
            {
                if (chr.Character.HealthPoints != null) UpdateTextSpawner(hpChanged);
            } 
        }
    }

    private void UpdateTextSpawner(CurrentHpChanged hpChanged)
    {
        int val = Mathf.RoundToInt((float)hpChanged.HealthChange);
        if (val != 0)
        {
            textSpawner.SpawnText(val.ToString());
        }
    }

    void UpdateBarFill(ICharacter character)
    {
        if (character.HealthPoints != null && hpBar)
        {
            var amount = character.HealthPoints.CurrentHp / character.HealthPoints.MaxHp;
            if (amount < 0) amount = 0;
            if (amount > 1) amount = 1;
            hpBar.SetFill((float)amount);
        }

    }

    void UpdateText(ICharacter character)
    {
        if (!text) return;
        if (character.HealthPoints != null)
        {
            text.text = Mathf.RoundToInt((float)character.HealthPoints.CurrentHp) + "/" + Mathf.RoundToInt((float)character.HealthPoints.MaxHp);
        }
    }
}
