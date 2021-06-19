
using UnityEngine;

public interface ICharacterAction : IHasTurnProgress
{
    Sprite Icon { get; }
    string Name { get; }
    ICharacter Target { get; }
    void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat);
    void PerformAction(ICharacter user, ICombatReader combat);
    void PostAction();
    void CancelAction();
}

public abstract class BaseCharacterAction : ICharacterAction
{
    private readonly Sprite icon;
    private readonly string name;
    private TurnProgress progress;
    private ICharacter target;

    protected BaseCharacterAction(string iconPath, string name)
    {
        this.icon = SingletonProvider.MainGameobjectLoader.GetPrefab<Sprite>(iconPath);
        this.name = name;
    }

    public TurnProgress TurnProgress => progress;

    public Sprite Icon => icon;

    public string Name => name;

    public ICharacter Target { get => target; }

    public void CancelAction()
    {
        progress = null;
        target = null;
    }

    public abstract void PerformAction(ICharacter user, ICombatReader combat);

    public void PostAction()
    {
        progress = null;
        target = null;
    }

    public virtual void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat)
    {
        progress = new TurnProgress();
        this.target = target;
    }
}