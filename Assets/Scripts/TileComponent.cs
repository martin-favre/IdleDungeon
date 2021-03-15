using System;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] GameObject northWall;
    [SerializeField] GameObject southWall;
    [SerializeField] GameObject eastWall;
    [SerializeField] GameObject westWall;

    public void SetTile(Tile tile)
    {
        this.tile = tile;
        CorrectWalls();
    }

    private void CorrectWalls()
    {
        northWall?.SetActive(!this.tile.GetWall(Directions.Direction.North));
        southWall?.SetActive(!this.tile.GetWall(Directions.Direction.South));
        eastWall?.SetActive(!this.tile.GetWall(Directions.Direction.East));
        westWall?.SetActive(!this.tile.GetWall(Directions.Direction.West));
    }
}