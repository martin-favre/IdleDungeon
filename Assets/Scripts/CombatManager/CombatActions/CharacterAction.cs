
using UnityEngine;

public interface ICharacterAction : IHasTurnProgress
{
    Sprite Icon { get; }
    string Name { get; }
    void StartChargingAction(ICharacter user, ICombatReader combat);
    void PerformAction(ICharacter user, ICombatReader combat);
    void PostAction();
}

public abstract class BaseCharacterAction : ICharacterAction
{
    private readonly Sprite icon;
    private readonly string name;
    private TurnProgress progress;

    protected BaseCharacterAction(string iconPath, string name)
    {
        this.icon = SingletonProvider.MainGameobjectLoader.GetPrefab<Sprite>(iconPath);
        this.name = name;
    }

    public TurnProgress TurnProgress => progress;

    public Sprite Icon => icon;

    public string Name => name;

    public abstract void PerformAction(ICharacter user, ICombatReader combat);

    public void PostAction()
    {
        progress = null;
    }

    public virtual void StartChargingAction(ICharacter user, ICombatReader combat)
    {
        progress = new TurnProgress();
    }
}