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
        var actions = character.CharacterActions;
        if (buttonIndex < actions.Length)
        {
            var action = GetAction();
            if (action != null)
            {
                turnProgressIndicator.gameObject.SetActive(true);
                turnProgressIndicator.SetTurnProgress(action);
                image.sprite = action.Icon;
                image.enabled = true;
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
        turnProgressIndicator.gameObject.SetActive(false);
    }

    public void OnButtonPressed(bool newVal)
    {
        var action = GetAction();
        if (action != null && targetType == CharacterStatBoxComponent.TargetType.Players)
        {
            if (newVal) // i.e. we pushed it down
            {
                clickSub = SingletonProvider.MainEventHandler.Subscribe(new[] { EventType.PlayerClickedEnemy, EventType.PlayerClickedNothing }, e =>
                  {
                      if (e is PlayerClickedEnemyEvent clkEn)
                      {
                          SingletonProvider.MainEventHandler.Publish(EventType.PlayerSelectedActionTarget, new PlayerSelectedActionTargetEvent(character, clkEn.Enemy, action));
                      }
                      else if (e is PlayerClickedNothingEvent)
                      {
                          toggle.isOn = false;
                          if (clickSub != null) clickSub.Dispose();
                          clickSub = null;
                      }
                  });
            }
        }
    }
}
