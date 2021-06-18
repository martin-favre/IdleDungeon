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

    private T LoadPrefab<T>(string path) where T : UnityEngine.Object
    {
        T obj = Resources.Load<T>(path);
        if (obj == null)
        {
            logger.Log("Could not load resource " + path, LogLevel.Error);
        }
        return obj;
    }

    readonly Dictionary<string, Object> prefabs = new Dictionary<string, Object>();

    public static IGameObjectLoader Instance { get => instance; }

    public T GetPrefab<T>(string name) where T : UnityEngine.Object
    {
        Object gameObject;
        prefabs.TryGetValue(name, out gameObject);
        if (gameObject == null)
        {
            gameObject = LoadPrefab<T>(name);
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