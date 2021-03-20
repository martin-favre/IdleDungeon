using System;
using UnityEngine;

[Serializable]
public class GoalTile : Tile
{
    [SerializeField]
    private Directions.Direction direction;

    public GoalTile()
    {
        SetAllWalls(true);

    }

    public Directions.Direction Direction { get => direction; set => direction = value; }

    public override string GetPrefabPath()
    {
        return "Prefabs/GoalTile";
    }
}