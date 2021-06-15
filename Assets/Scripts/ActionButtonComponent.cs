using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ActionButtonComponent : MonoBehaviour
{
    [SerializeField]
    private UITurnProgressIndicator turnProgressIndicator;
    [SerializeField]
    private int buttonIndex;
    Image image;
    ICharacterAction action;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        turnProgressIndicator.gameObject.SetActive(false);
    }
    public void SetCharacter(ICharacter character)
    {
        var actions = character.CharacterActions;
        if (buttonIndex < actions.Length)
        {
            action = actions[buttonIndex];
            turnProgressIndicator.gameObject.SetActive(true);
            turnProgressIndicator.SetTurnProgress(action);
            image.sprite = action.Icon;
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
            turnProgressIndicator.gameObject.SetActive(false);
        }
    }
}
