using UnityEngine;

public interface IGameObjectLoader {
     T GetPrefab<T>(string name) where T : class;
     GameObject Instantiate(GameObject original);
}