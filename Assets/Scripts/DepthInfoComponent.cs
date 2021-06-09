using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class DepthInfoComponent : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text text;
    LilLogger logger = new LilLogger(typeof(DepthInfoComponent).ToString());
    IObserver<IPersistentStorageUpdateEvent> observer;
    void Start()
    {
        if (text == null) logger.Log("Missing field " + nameof(text), LogLevel.Warning);
        UpdateText();
        observer = new KeyObserver<string, IPersistentStorageUpdateEvent>(SingletonProvider.MainDataStorage, Constants.currentLevelKey, e => UpdateText());
    }

    void Update()
    {

    }

    void UpdateText()
    {
        if (text)
        {
            int depth = SingletonProvider.MainDataStorage.GetInt(Constants.currentLevelKey, 0);
            text.text = "Depth: " + depth + "\n" + 
            "Exp bonus: " + Mathf.RoundToInt((float)(100*Mathf.Pow(1.07f, depth)-100)) + "%"; // hardcoded from LevelGeneratedCharacter to show concept
        }
    }
}
