using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterStatBoxComponent : MonoBehaviour
{
    static readonly string[] names = new string[] {
        "Steve",
        "Bob",
        "Joe",
        "Eric"
    };
    [SerializeField]
    private TMP_Text characterName;
    [SerializeField]
    private HealthbarComponent healthbarComponent;
    [SerializeField]
    private int playerIndex;

    private int oldPlayerIndex;
    void Start()
    {
        SetPlayerIndex(playerIndex);
    }

    void SetPlayerIndex(int newPlayerIndex)
    {
        var chars = PlayerCharacters.Instance.GetAllPlayersChars();
        if (playerIndex < chars.Length)
        {
            if (characterName)
            {
                characterName.enabled = true;
                characterName.text = names[playerIndex];
            }
            if (healthbarComponent)
            {
                healthbarComponent.gameObject.SetActive(true);
                healthbarComponent.PlayerIndex = playerIndex;
            }
        }
        else
        {
            if (characterName) characterName.enabled = false;
            if (healthbarComponent) healthbarComponent.gameObject.SetActive(false);
        }
        oldPlayerIndex = playerIndex;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIndex != oldPlayerIndex) // Just to make it editable from the editor
        {
            SetPlayerIndex(playerIndex);
        }
    }
}
