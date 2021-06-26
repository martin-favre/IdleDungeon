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
    [SerializeField]
    private ActionButtonComponent[] actionButtonComponents;

    [SerializeField]
    private BuffListComponent buffListComponent;
    Subscription<EventType> combatSubscription;

    public int TargetIndex { get => targetIndex; }

    public bool IsPlayer()
    {
        return targetType == TargetType.Players;
    }

    void Start()
    {
        UpdateIndex(null);
        combatSubscription = CentralEventPublisher.Instance.Subscribe(new[] { EventType.CombatStarted, EventType.CombatEnded }, e =>
         {
             UpdateIndex(e);
         });
    }

    void UpdateIndex(IEvent e)
    {
        var character = GetCharacter();
        foreach (var actionButton in actionButtonComponents)
        {
            actionButton.SetCharacter(character, targetType);
        }

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
            if (buffListComponent)
            {
                buffListComponent.SetCharacter(character);
            }
        }
        else
        {
            if (characterName) characterName.enabled = false;
            if (healthbarComponent) healthbarComponent.gameObject.SetActive(false);
        }
    }

    public ICharacter GetCharacter()
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
            var reader = SingletonProvider.MainCombatManager.CombatReader;
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
}
