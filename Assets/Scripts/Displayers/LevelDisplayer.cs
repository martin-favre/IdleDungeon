
using UnityEngine;

public class LevelDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook valueHook;
    KeyObserver<IPersistentStorageUpdateEvent, string> observer;
    private void Start()
    {
        valueHook = SimpleValueDisplayer.Instance.RegisterValue();
        UpdateText(PlayerPrefsReader.Instance.GetInt(Constants.currentLevelKey, 0));
        observer = new KeyObserver<IPersistentStorageUpdateEvent, string>(
            PlayerPrefsReader.Instance,
            Constants.currentLevelKey,
            (evt) =>
            {
                var intEvt = ((IntPersistentStorageUpdateEvent)evt);
                if(intEvt != null) UpdateText(intEvt.Value);
            });
    }

    private void UpdateText(int newVal) {
        valueHook.UpdateValue("Currently on level: " + newVal.ToString());
    }

    private void Update()
    {

    }
}
