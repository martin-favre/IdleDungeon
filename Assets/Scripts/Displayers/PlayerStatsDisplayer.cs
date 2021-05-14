using UnityEngine;

public class PlayerStatsDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<IPlayerCharacterUpdateEvent> observer;

    void Start()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        var chars = PlayerCharacters.Instance.GetAllPlayersChars();
        if (chars.Length > 0)
        {
            UpdateText(chars[0].Attributes);
        }
        observer = new SimpleObserver<IPlayerCharacterUpdateEvent>(PlayerCharacters.Instance, (e) =>
        {
            if (e is PlayerCharacterAttributeUpdateEvent) UpdateText((e as PlayerCharacterAttributeUpdateEvent).Attributes);
        });
    }

    private void UpdateText(ICombatAttributes attributes)
    {
        displayer.UpdateValue("Player HP: " + Mathf.RoundToInt((float)attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)attributes.MaxHp));
    }
}