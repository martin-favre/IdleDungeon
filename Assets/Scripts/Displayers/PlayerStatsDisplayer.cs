using PubSubSystem;
using UnityEngine;

public class PlayerStatsDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    Subscription<EventType> subscription;

    void Start()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        var chars = SingletonProvider.MainPlayerRoster.GetAllPlayersChars();
        if (chars.Length > 0)
        {
            UpdateText(chars[0].Attributes);
            subscription = MainEventHandler.Instance.Subscribe(EventType.CharacterAttributeChanged, e =>
            {
                if (e is AttributeChangedEvent attr) UpdateText(attr.Character.Attributes);
            });
        }
    }

    private void UpdateText(ICombatAttributes attributes)
    {
        if (attributes is IHealthPoints hp)
        {
            displayer.UpdateValue("Player HP: " + Mathf.RoundToInt((float)hp.CurrentHp) + "/" + Mathf.RoundToInt((float)hp.MaxHp));
        }
    }
}