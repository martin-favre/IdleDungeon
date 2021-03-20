using System;
[Serializable]
public class StartTile : Tile
{
    private Directions.Direction direction;
    public StartTile()
    {
        SetAllWalls(true);
    }

    public Directions.Direction Direction { get => direction; set => direction = value; }
    public override string GetPrefabPath()
    {
        return "Prefabs/StartTile";
    }

}