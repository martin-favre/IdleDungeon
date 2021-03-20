
using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeComponent : MonoBehaviour
{

    IGridMap maze;
    List<GameObject> tiles;


    
    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void Setup(IGridMap maze)
    {
        this.maze = maze;
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        tiles = new List<GameObject>();
        GameObject tilePrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Tile");
        Helpers.DoForAll(maze.Size, (pos) => {
            GameObject gObj = PrefabLoader.Instance.Instantiate(tilePrefab) as GameObject;
            gObj.transform.parent = transform;
            gObj.transform.position = new Vector3(pos.x*Constants.tileSize.x, 0, pos.y*Constants.tileSize.x);
            gObj.GetComponent<TileComponent>().SetTile(maze.GetTile(pos));
        });
    }
}