using System.Collections.Generic;
using Logging;
using UnityEngine;

class PrefabLoader : IGameObjectLoader
{
    private readonly LilLogger logger = new LilLogger("PrefabLoader");

    static IGameObjectLoader instance;

    static PrefabLoader()
    {
        instance = new PrefabLoader();
    }

    private Object LoadPrefab(string path)
    {
        Object obj = Resources.Load(path) as Object;
        if (obj == null)
        {
            logger.Log("Could not load resource " + path, LogLevel.Error);
        }
        return obj;
    }

    readonly Dictionary<string, Object> prefabs = new Dictionary<string, Object>();

    public static IGameObjectLoader Instance { get => instance; }

    public T GetPrefab<T>(string name) where T : class
    {
        Object gameObject;
        prefabs.TryGetValue(name, out gameObject);
        if (gameObject == null)
        {
            gameObject = LoadPrefab(name);
            prefabs[name] = gameObject;
        }
        return gameObject as T;
    }

    public void ReplaceInstance(IGameObjectLoader obj) {
        instance = obj;
    }

    public GameObject Instantiate(GameObject original) 
    {
        return GameObject.Instantiate(original) as GameObject;
    }
}