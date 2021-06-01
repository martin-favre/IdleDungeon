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
    private DamageTextSpawnerComponent textSpawner;

    Subscription<CharacterUpdateEventType> subscription;

    IGuid targetGuid;

    public void SetAttributes(ICharacter character)
    {
        targetGuid = character.UniqueId;
        subscription = CharacterEventPublisher.Instance.Subscribe(new[] { CharacterUpdateEventType.AttributeChanged, CharacterUpdateEventType.CurrentHpChanged }, HandleEvent);
        UpdateBarFill(character.Attributes);
        UpdateText(character.Attributes);
        UpdateTextSpawner(character);
    }

    private void HandleEvent(IEvent ev)
    {
        if (ev is CharacterUpdateEvent chr && chr.Character.UniqueId.Equals(targetGuid))
        {
            UpdateBarFill(chr.Character.Attributes);
            UpdateText(chr.Character.Attributes);
            // textSpawner handles its own events
        }
    }

    private void UpdateTextSpawner(ICharacter character)
    {
        textSpawner.SetCharacter(character);
    }

    void UpdateBarFill(ICombatAttributes attributes)
    {
        var amount = attributes.CurrentHp / attributes.MaxHp;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
        hpBar.fillAmount = (float)amount;

    }

    void UpdateText(ICombatAttributes attributes)
    {
        if (!text) return;
        text.text = Mathf.RoundToInt((float)attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)attributes.MaxHp);
    }
}
