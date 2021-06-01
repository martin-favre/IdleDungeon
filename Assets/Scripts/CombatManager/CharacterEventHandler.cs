
using System;
using PubSubSystem;

public enum CharacterUpdateEventType
{
    NameUpdated,
    AttributeChanged,
    CurrentHpChanged,
}

public class CharacterEventPublisher : EventPublisher<CharacterUpdateEventType>
{
    static readonly CharacterEventPublisher instance;

    public static CharacterEventPublisher Instance => instance;

    static CharacterEventPublisher()
    {
        instance = new CharacterEventPublisher();
    }
}