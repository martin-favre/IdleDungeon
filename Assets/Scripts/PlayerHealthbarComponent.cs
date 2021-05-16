using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerHealthbarComponent : MonoBehaviour
{

    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private int playerIndex;

    [SerializeField]
    private TMP_Text text;

    private SimpleObserver<ICharacterUpdateEvent> observer;

    public int PlayerIndex { get => playerIndex; set => UpdatePlayerIndex(value); }

    void Start()
    {
        UpdatePlayerIndex(playerIndex);
    }

    void HandleEvent(ICharacterUpdateEvent ev)
    {
        if (ev is AttributeChangedEvent attr) UpdateBarFill(attr.PlayerAttributes);
    }

    private void UpdatePlayerIndex(int newIndex)
    {
        var characters = SingletonProvider.MainPlayerRoster.GetAllPlayersChars();
        if (newIndex >= characters.Length) return;
        if (observer != null) observer.Dispose();
        observer = new SimpleObserver<ICharacterUpdateEvent>(characters[newIndex], e =>
        {
            if (e is AttributeChangedEvent attr) UpdateBarFill(attr.PlayerAttributes);
        });
        playerIndex = newIndex;
        UpdateBarFill(SingletonProvider.MainPlayerRoster.GetAllPlayersChars()[playerIndex].Attributes);
    }

    void UpdateBarFill(ICombatAttributes attributes)
    {
        var amount = attributes.CurrentHp / attributes.MaxHp;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
        hpBar.fillAmount = (float)amount;
        UpdateText(attributes);
    }

    void UpdateText(ICombatAttributes attributes)
    {
        if (!text) return;
        text.text = Mathf.RoundToInt((float)attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)attributes.MaxHp);
    }

    void Update()
    {

    }
}
