using UnityEngine;

public class PlayerStatsDisplayer : MonoBehaviour {
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<PlayerCharacterUpdateEvent> observer;

    void Awake() {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        var chars = PlayerCharacters.Instance.GetAllPlayersChars();
        if(chars.Length > 0) {
            UpdateText(chars[0]);
        }
        observer = new SimpleObserver<PlayerCharacterUpdateEvent>(PlayerCharacters.Instance, (e) =>{
            UpdateText(e.Character);
        });
    }

    private void UpdateText(PlayerCharacter character)
    {
        displayer.UpdateValue("Player HP: " + character.Attributes.Hp);
    }
}