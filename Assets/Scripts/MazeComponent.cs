
using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeComponent : MonoBehaviour
{

    IGridMap maze;
    List<GameObject> tiles;
    private void Start()
    {
        SetMaze(new RecursiveBacktracker().GenerateMap(new Vector2Int(20, 20), 10));
    }

    private void Update()
    {

    }

    public void SetMaze(IGridMap maze)
    {
        this.maze = maze;
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        tiles = new List<GameObject>();
        GameObject tilePrefab = PrefabLoader.GetPrefab<GameObject>("Prefabs/Tile");
        Helpers.DoForAll(maze.Size, (pos) => {
            GameObject gObj = Instantiate(tilePrefab, transform) as GameObject;
            gObj.transform.position = new Vector3(pos.x*10, 0, pos.y*10);
            gObj.GetComponent<TileComponent>().SetTile(maze.GetTile(pos));
        });
    }
}