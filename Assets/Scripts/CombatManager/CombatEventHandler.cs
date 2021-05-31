
using PubSubSystem;

public class CombatEventPublisher : SimpleEventPublisher
{
    static readonly CombatEventPublisher instance;

    public static CombatEventPublisher Instance => instance;

    static CombatEventPublisher()
    {
        instance = new CombatEventPublisher();
    }
}