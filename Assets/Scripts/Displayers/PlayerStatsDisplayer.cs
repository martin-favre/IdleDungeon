using UnityEngine;

public class PlayerStatsDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<ICharacterUpdateEvent> observer;

    void Start()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        var chars = PlayerRoster.Instance.GetAllPlayersChars();
        if (chars.Length > 0)
        {
            UpdateText(chars[0].Attributes);
            observer = new SimpleObserver<ICharacterUpdateEvent>(chars[0], e =>
            {
                if (e is AttributeChangedEvent) UpdateText((e as AttributeChangedEvent).PlayerAttributes);
            });
        }
    }

    private void UpdateText(ICombatAttributes attributes)
    {
        displayer.UpdateValue("Player HP: " + Mathf.RoundToInt((float)attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)attributes.MaxHp));
    }
}