
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
    private readonly float baseActionTimeS;
    private TurnProgress progress;
    private ICharacter target;
    private string icon1;

    protected BaseCharacterAction(string iconPath, string name, float baseActionTimeS)
    {
        this.icon = SingletonProvider.MainGameobjectLoader.GetPrefab<Sprite>(iconPath);
        this.name = name;
        this.baseActionTimeS = baseActionTimeS;
    }

    public Sprite Icon => icon;

    public string Name => name;

    public ICharacter Target { get => target; }

    public TurnProgress TurnProgress => progress;

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
        var actionTime = baseActionTimeS / user.Attributes.Speed;
        progress = new TurnProgress(actionTime);
        this.target = target;
    }
}