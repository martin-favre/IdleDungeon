
using UnityEngine;


public abstract class BaseCharacterAction : ICharacterAction
{
    private readonly Sprite icon;
    private readonly string name;
    private readonly float baseActionTimeS;
    private TurnProgress progress;
    private ICharacter target;

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

    public void AfterAction()
    {
        progress = null;
        target = null;
    }

    public void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat)
    {
        var actionTime = baseActionTimeS / user.Attributes.Speed;
        progress = new TurnProgress(actionTime);
        this.target = target;
    }
}