using Logging;
using PubSubSystem;
using TMPro;
using UnityEngine;

public class PoppingTextSpawnerComponent : MonoBehaviour
{
    const string textPrefab = "Prefabs/PoppingText";
    GameObject textPrefabObj;
    ICharacter character;
    Subscription<EventType> subscription;

    IGuid targetGuid;

    readonly static LilLogger logger = new LilLogger(typeof(PoppingTextSpawnerComponent).ToString());

    void Start()
    {
        textPrefabObj = SingletonProvider.MainGameobjectLoader.GetPrefab<GameObject>(textPrefab);
    }

    public void SpawnText(string text)
    {
        GameObject obj = SingletonProvider.MainGameobjectLoader.Instantiate(textPrefabObj);
        TMP_Text tmpText = obj.GetComponent<TMP_Text>();
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        if (!tmpText) return;
        tmpText.text = text;
    }


}