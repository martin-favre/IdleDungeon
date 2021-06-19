using System;
using UnityEngine;

public interface IPlayerMover
{
    Vector3 WorldPosition { get; }
    void RotateTowards(Vector2Int posToLookAt, Action notifyDone);
    void MoveTowards(Vector2Int posToGoTo, Action notifyDone);
}