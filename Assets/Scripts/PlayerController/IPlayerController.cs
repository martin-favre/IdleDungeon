using UnityEngine;

public interface IPlayerController
{
    bool HasNextStep();
    Vector2Int GetNextStep();
    void NotifyPathFinished();
    void RequestLookAt(Vector2Int position);

    void RequestMoveTo(Vector2Int position);

    ICombatManager CombatManager { get; }

    Vector2Int Position { get; set; }
}