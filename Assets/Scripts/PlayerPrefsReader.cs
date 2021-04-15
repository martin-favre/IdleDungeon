using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsReader : IPersistentDataStorage
{
    private static PlayerPrefsReader instance;
    public static PlayerPrefsReader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerPrefsReader();
                instance.ClearStorage(); // clear all during development
            }
            return instance;
        }
    }

    Dictionary<string, List<IObserver<IPersistentStorageUpdateEvent>>> observers = new Dictionary<string, List<IObserver<IPersistentStorageUpdateEvent>>>();

    public float GetFloat(string key)
    {
        return GetFloat(key, 0);
    }

    public float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        NotifyObservers(key, new FloatPersistentStorageUpdateEvent(value));
    }

    public int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    public int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        NotifyObservers(key, new IntPersistentStorageUpdateEvent(value));

    }

    public string GetString(string key)
    {
        return GetString(key, "");
    }

    public string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }


    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        NotifyObservers(key, new StringPersistentStorageUpdateEvent(value));
    }

    public void ClearStorage()
    {
        PlayerPrefs.DeleteAll();
    }

    private void NotifyObservers(string key, IPersistentStorageUpdateEvent evt)
    {
        List<IObserver<IPersistentStorageUpdateEvent>> keyObservers;
        bool success = observers.TryGetValue(key, out keyObservers);
        if(!success) return;
        foreach (var observer in keyObservers)
        {
            observer.OnNext(evt);
        }
    }

    // key is which value you want to subscribe to
    // You'll get an OnNext called whenever this value is written to
    public IDisposable Subscribe(IObserver<IPersistentStorageUpdateEvent> observer, string key)
    {
        return new KeyUnsubscriber<IPersistentStorageUpdateEvent, string>(observers, key, observer);
    }
}