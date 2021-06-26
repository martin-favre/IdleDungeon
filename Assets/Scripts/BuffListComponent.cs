using System.Collections;
using System.Collections.Generic;
using PubSubSystem;
using UnityEngine;

public class BuffListComponent : MonoBehaviour
{


    [SerializeField]
    private BuffIconComponent[] icons;
    private Subscription<EventType> subscription;
    ICharacter character;
    private void Awake()
    {
        subscription = SingletonProvider.MainEventPublisher.Subscribe(new[] { EventType.BuffExpired, EventType.BuffApplied }, OnEvent);

        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(false);
        }
    }

    private void OnEvent(IEvent e)
    {
        if (character != null)
        {
            UpdateIcons(character.Buffs);
        }
    }

    public void SetCharacter(ICharacter character)
    {
        UpdateIcons(character.Buffs);
        this.character = character;
    }

    private void UpdateIcons(IBuff[] buffs)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            bool isUsed = i < buffs.Length;
            icons[i].gameObject.SetActive(isUsed);
            if (isUsed)
            {
                icons[i].SetBuff(buffs[i]);
            }
        }
    }
}
