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



    void Awake()
    {
        HideIcons();
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
        if(targetType == CharacterStatBoxComponent.TargetType.Enemies) toggle.interactable = false;
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
        if (e is PlayerClickedEnemyEvent clkEn)
        {
            var action = GetAction();
            SingletonProvider.MainEventHandler.Publish(EventType.PlayerSelectedActionTarget, new PlayerSelectedActionTargetEvent(character, clkEn.Enemy, action));
            actionSub = SingletonProvider.MainEventHandler.Subscribe(EventType.CombatAction, OnPlayerTookAction);
        }
        toggle.isOn = false;
        clickSub.Dispose();
    }

    private void OnPlayerTookAction(IEvent e)
    {
        // So, we've clicked the enemy
        // And now the action was taken

        if (e is CombatActionEvent ev)
        {
            if (ev.Action == GetAction())
            {
                actionSub.Dispose();
            }
        }
    }

    public void OnButtonPressed(bool newVal)
    {
        var action = GetAction();
        if (action != null && action is IHasTarget)
        {
            if (newVal) // i.e. we pushed it down
            {
                if(clickSub != null) clickSub.Dispose();
                clickSub = SingletonProvider.MainEventHandler.Subscribe(new[] { EventType.PlayerClickedEnemy, EventType.PlayerClickedNothing }, OnPlayerClickedEnemy);
            }
        }
    }
}