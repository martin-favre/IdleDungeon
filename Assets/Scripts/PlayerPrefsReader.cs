using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logging;

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
            }
            return instance;
        }
    }

    static readonly LilLogger logger = new LilLogger(typeof(PlayerPrefsReader).ToString());

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
        logger.Log("Setting " + key + " to " + value);
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
        logger.Log("Setting " + key + " to " + value);
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
        logger.Log("Setting " + key + " to " + value);
    }

    public void ClearStorage()
    {
        PlayerPrefs.DeleteAll();
        NotifyClearedStorage();
        logger.Log("Clearing storage");
    }

    private void NotifyClearedStorage()
    {
        var ev = new DataClearedUpdateEvent();
        // Need to clone the old entries to allow things to unsub
        var oldObserverDict = observers.ToDictionary(entry => entry.Key, entry => entry.Value);
        foreach (var list in oldObserverDict)
        {
            var oldList = list.Value.ToArray();
            foreach (var observer in oldList)
            {
                observer.OnNext(ev);
            }
        }
    }

    private void NotifyObservers(string key, IPersistentStorageUpdateEvent evt)
    {
        List<IObserver<IPersistentStorageUpdateEvent>> keyObservers;
        bool success = observers.TryGetValue(key, out keyObservers);
        if (!success) return;
        foreach (var observer in keyObservers)
        {
            observer.OnNext(evt);
        }
    }

    // key is which value you want to subscribe to
    // You'll get an OnNext called whenever this value is written to
    public IDisposable Subscribe(string key, IObserver<IPersistentStorageUpdateEvent> observer)
    {
        return new KeyUnsubscriber<string, IPersistentStorageUpdateEvent>(observers, key, observer);
    }
}