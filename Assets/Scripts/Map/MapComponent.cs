
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour
{

    IMap map;
    List<GameObject> tiles;


    
    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void Setup(IMap map) 
    {
        this.map = map;
        GenerateMap();
    }

    private void GenerateMap()
    {
        tiles = new List<GameObject>();
        GameObject tilePrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Tile");
        Helpers.DoForAll(map.Size, (pos) => {
            GameObject gObj = PrefabLoader.Instance.Instantiate(tilePrefab) as GameObject;
            gObj.transform.parent = transform;
            gObj.transform.position = new Vector3(pos.x*Constants.tileSize.x, 0, pos.y*Constants.tileSize.x);
            gObj.GetComponent<TileComponent>().SetTile(map.GetTile(pos));
        });
    }
}