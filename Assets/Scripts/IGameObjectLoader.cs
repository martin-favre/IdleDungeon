using UnityEngine;

public interface IGameObjectLoader {
     T GetPrefab<T>(string name) where T : UnityEngine.Object;
     GameObject Instantiate(GameObject original);
}