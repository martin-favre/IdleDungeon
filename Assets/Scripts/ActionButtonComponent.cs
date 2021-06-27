using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PubSubSystem;

public class ActionButtonComponent : MonoBehaviour
{
    [SerializeField]
    private UITurnProgressIndicator turnProgressIndicator;
    [SerializeField]
    private int buttonIndex;
    CharacterStatBoxComponent.TargetType targetType;
    ICharacter character;
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    Image actionIcon;

    Subscription<EventType> clickSub;
    Subscription<EventType> actionSub;
    Subscription<EventType> combatSub;

    void Awake()
    {
        HideIcons();
        combatSub = SingletonProvider.MainEventPublisher.Subscribe(new[] { EventType.CombatEnded, EventType.CombatStarted }, OnCombatChanged);
        toggle.interactable = false;
    }

    void OnCombatChanged(IEvent ev)
    {
        if (ev is CombatStartedEvent started && character != null && character.IsPlayer)
        {
            toggle.interactable = true;
        }
        else
        {
            toggle.interactable = false;
        }
    }

    private ICharacterAction GetAction()
    {
        if (character != null && buttonIndex < character.CharacterActions.Length)
        {
            return character.CharacterActions[buttonIndex];
        }
        return null;
    }

    public void SetCharacter(ICharacter character, CharacterStatBoxComponent.TargetType targetType)
    {
        this.character = character;
        if (character == null)
        {
            HideIcons();
            return;
        }
        this.targetType = targetType;
        var actions = character.CharacterActions;
        if (buttonIndex < actions.Length)
        {
            var action = GetAction();
            if (action != null)
            {
                actionIcon.sprite = action.Icon;
                toggle.enabled = true;
                actionIcon.enabled = true;
                turnProgressIndicator.SetTurnProgressOwner(action);
            }
        }
        else
        {
            HideIcons();
        }
    }

    private void HideIcons()
    {
        toggle.enabled = false;
        actionIcon.enabled = false;
    }

    private void OnPlayerClickedEnemy(IEvent e)
    {
        if (e is PlayerClickedTargetEvent clkEn)
        {
            var action = GetAction();
            SingletonProvider.MainEventPublisher.Publish(EventType.PlayerSelectedActionTarget, new PlayerSelectedActionTargetEvent(character, clkEn.Enemy, action));
        }
        toggle.isOn = false;
        clickSub.Dispose();
        clickSub = null;
    }

    public void OnButtonPressed(bool newVal)
    {
        var action = GetAction();
        if (action != null && character != null && !character.IsDead())
        {
            if (newVal) // i.e. we pushed it down
            {
                if (clickSub == null)
                {
                    clickSub = SingletonProvider.MainEventPublisher.Subscribe(new[] { EventType.PlayerClickedTarget, EventType.PlayerClickedNothing }, OnPlayerClickedEnemy);
                }
            }
        }
    }
}
