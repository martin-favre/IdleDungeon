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
