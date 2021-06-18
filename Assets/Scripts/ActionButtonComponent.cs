using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PubSubSystem;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Toggle))]
public class ActionButtonComponent : MonoBehaviour
{
    [SerializeField]
    private UITurnProgressIndicator turnProgressIndicator;
    [SerializeField]
    private int buttonIndex;
    Image image;
    CharacterStatBoxComponent.TargetType targetType;
    ICharacter character;

    Toggle toggle;

    Subscription<EventType> clickSub;
    Subscription<EventType> actionSub;

    void Awake()
    {
        image = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
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
                image.sprite = action.Icon;
                image.enabled = true;
                turnProgressIndicator.SetTurnProgressOwner(action);
            }
        }
        else
        {
            HideIcons();
        }
    }

    private void Update()
    {
    }

    private void HideIcons()
    {
        image.enabled = false;
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
