using UnityEngine;

public interface ICharacterAction : IHasTurnProgress
{
    Sprite Icon { get; }
    ICharacter Target { get; }
    void StartChargingAction(ICharacter user, ICharacter target, ICombatReader combat);
    void PerformAction(ICharacter user, ICombatReader combat);
    void AfterAction();
    void CancelAction();
    DamageConfig Damage{get;}
    bool TargetsEnemies{get;}
    bool TargetsAllies{get;}
    float BaseActionTime{get;}
}
