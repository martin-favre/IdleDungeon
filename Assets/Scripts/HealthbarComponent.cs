using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PubSubSystem;

public class HealthbarComponent : MonoBehaviour
{

    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private PoppingTextSpawnerComponent textSpawner;

    Subscription<EventType> subscription;

    IGuid targetGuid;

    public void SetAttributes(ICharacter character)
    {
        targetGuid = character.UniqueId;
        subscription = MainEventHandler.Instance.Subscribe(new[] { EventType.CharacterAttributeChanged, EventType.CharacterCurrentHpChanged }, HandleEvent);
        if (character.Attributes is IHealthPoints hp)
        {
            UpdateBarFill(character.Attributes);
            UpdateText(character.Attributes);
        } else {
            gameObject.SetActive(false);
        }
    }

    private void HandleEvent(IEvent ev)
    {
        if (ev is CharacterUpdateEvent chr && chr.Character.UniqueId.Equals(targetGuid))
        {
            UpdateBarFill(chr.Character.Attributes);
            UpdateText(chr.Character.Attributes);
            if(ev is CurrentHpChanged hpChanged) {
                if(chr.Character.Attributes is IHealthPoints hp) UpdateTextSpawner(hpChanged);
            }
        }
    }

    private void UpdateTextSpawner(CurrentHpChanged hpChanged)
    {
        int val = Mathf.RoundToInt((float)hpChanged.HealthChange);
        if(val != 0) {
            textSpawner.SpawnText(val.ToString());
        }
    }

    void UpdateBarFill(ICombatAttributes attributes)
    {
        if (attributes is IHealthPoints hp)
        {
            var amount = hp.CurrentHp / hp.MaxHp;
            if (amount < 0) amount = 0;
            if (amount > 1) amount = 1;
            hpBar.fillAmount = (float)amount;
        }

    }

    void UpdateText(ICombatAttributes attributes)
    {
        if (!text) return;
        if (attributes is IHealthPoints hp)
        {
            text.text = Mathf.RoundToInt((float)hp.CurrentHp) + "/" + Mathf.RoundToInt((float)hp.MaxHp);
        }
    }
}
