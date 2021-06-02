using Logging;
using PubSubSystem;
using TMPro;
using UnityEngine;

public class DamageTextSpawnerComponent : MonoBehaviour
{
    const string textPrefab = "Prefabs/DmgText";
    GameObject textPrefabObj;
    ICharacter character;
    Subscription<CharacterUpdateEventType> subscription;

    IGuid targetGuid;

    readonly static LilLogger logger = new LilLogger(typeof(DamageTextSpawnerComponent).ToString());

    void Start()
    {
        textPrefabObj = SingletonProvider.MainGameobjectLoader.GetPrefab<GameObject>(textPrefab);
    }
    public void SetCharacter(ICharacter character)
    {
        this.character = character;
        targetGuid = character.UniqueId;
        if(subscription != null) subscription.Dispose();
        subscription = CharacterEventPublisher.Instance.Subscribe(CharacterUpdateEventType.CurrentHpChanged, OnEvent);
        logger.Log("Setting character " + character.Name + " guid: " + character.UniqueId);
    }

    void OnEvent(IEvent ev)
    {
        if (ev is CurrentHpChanged attr)
        {
            logger.Log("UpdateEvent I am: " + character.Name + " guid: " + character.UniqueId + " event for: " + attr.Character.Name + " guid: " + attr.Character.UniqueId);
            if (attr.Character.UniqueId.Equals(targetGuid))
            {
                int lostHp = Mathf.RoundToInt((float)attr.HealthChange);
                if (lostHp != 0)
                {
                    SpawnText(lostHp.ToString());
                }
            }
        }
    }

    void Update()
    {
        // SpawnText("Hello");
    }

    void SpawnText(string text)
    {
        GameObject obj = SingletonProvider.MainGameobjectLoader.Instantiate(textPrefabObj);
        TMP_Text tmpText = obj.GetComponent<TMP_Text>();
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        if (!tmpText) return;
        tmpText.text = text;
    }


}