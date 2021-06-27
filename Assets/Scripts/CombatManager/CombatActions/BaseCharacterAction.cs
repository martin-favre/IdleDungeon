
using Logging;
using UnityEngine;


public abstract class BaseCharacterAction : ICharacterAction
{
    private readonly Sprite icon;
    private readonly string iconPath;
    private readonly float baseActionTime;
    private readonly DamageConfig damageConfig;
    private readonly bool targetsEnemies;
    private readonly bool targetsAllies;
    private TurnProgress progress;
    private ICharacter target;
    static readonly LilLogger logger = new LilLogger(typeof(BaseCharacterAction).ToString());

    protected BaseCharacterAction(string iconPath, float baseActionTime, DamageConfig damageConfig, bool targetsEnemies, bool targetsAllies)
    {
        this.icon = SingletonProvider.MainGameobjectLoader.GetPrefab<Sprite>(iconPath);
        this.iconPath = iconPath;
        this.baseActionTime = baseActionTime;
        this.damageConfig = damageConfig;
        this.targetsEnemies = targetsEnemies;
        this.targetsAllies = targetsAllies;
    }

    public Sprite Icon => icon;

    public ICharacter Target { get => target; }

    public TurnProgress TurnProgress => progress;

    public DamageConfig Damage => damageConfig;

    public bool TargetsEnemies => targetsEnemies;

    public bool TargetsAllies => targetsAllies;

    public float BaseActionTime => baseActionTime;

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

    public virtual void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat)
    {
        bool sameTeam = user.IsPlayer == target.IsPlayer;
        if (sameTeam && !TargetsAllies || !sameTeam && !TargetsEnemies)
        {
            logger.Log("Trying to charge an action towards a non-matching target, sameTeam: " + sameTeam + " TargetsEnemies" + targetsEnemies + " TargetsAllies: " + TargetsAllies, LogLevel.Warning);
            SingletonProvider.MainEventPublisher.Publish(EventType.CharacterActionCancelled, new CharacterActionCancelledEvent(user, this));
            return;
        }
        var actionTime = BaseActionTime / user.Attributes.Speed;
        progress = new TurnProgress(actionTime);
        this.target = target;
    }
}