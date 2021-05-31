using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PubSubSystem;

public class CharacterStatBoxComponent : MonoBehaviour
{
    public enum TargetType
    {
        Players,
        Enemies
    }
    [SerializeField]
    private TMP_Text characterName;
    [SerializeField]
    private HealthbarComponent healthbarComponent;

    [SerializeField]
    private TargetType targetType;


    [SerializeField]
    private int targetIndex;
    private int oldTargetIndex;
    SimpleObserver<ICharacterUpdateEvent> characterObserver;
    Subscription<int> combatSubscription;
    void Start()
    {
        UpdateIndex(targetIndex);
        combatSubscription = CombatEventPublisher.Instance.Subscribe(e => UpdateIndex(targetIndex));
    }

    void UpdateIndex(int index)
    {
        var character = GetCharacter(targetIndex);
        if (character != null)
        {
            if (characterName)
            {
                characterName.enabled = true;
                characterName.text = character.Name;
            }
            if (healthbarComponent)
            {
                healthbarComponent.gameObject.SetActive(true);
                healthbarComponent.SetAttributes(character);
            }
            if (characterObserver != null) characterObserver.Dispose();
            characterObserver = new SimpleObserver<ICharacterUpdateEvent>(character, e => UpdateIndex(targetIndex));
        }
        else
        {
            if (characterName) characterName.enabled = false;
            if (healthbarComponent) healthbarComponent.gameObject.SetActive(false);
        }

        oldTargetIndex = index;
    }

    ICharacter GetCharacter(int targetIndex)
    {
        if (targetType == TargetType.Players)
        {
            var chars = SingletonProvider.MainPlayerRoster.GetAllPlayersChars();
            if (targetIndex < chars.Length)
            {
                return chars[targetIndex];
            }
            else
            {
                return null;
            }
        }
        else
        {
            var reader = CombatManager.Instance.GetReader();
            if (reader != null)
            {
                var enemies = reader.GetEnemies();
                if (targetIndex < enemies.Length)
                {
                    return enemies[targetIndex];
                }
                else
                {
                    return null;
                }

            }
        }
        return null;
    }

    public void OpenUpgradeTab()
    {
        if(targetType == TargetType.Players) {
            UpgradePanelComponent.Instance.OpenPanel(targetIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetIndex != oldTargetIndex) // Just to make it editable from the editor
        {
            UpdateIndex(targetIndex);
        }
    }
}
