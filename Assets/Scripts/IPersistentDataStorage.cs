

using System;
using System.Reflection;

public interface IPersistentStorageUpdateEvent { }

public class FloatPersistentStorageUpdateEvent : IPersistentStorageUpdateEvent
{
    private readonly float value;

    public FloatPersistentStorageUpdateEvent(float value)
    {
        this.value = value;
    }

    public float Value => value;
}

public class IntPersistentStorageUpdateEvent : IPersistentStorageUpdateEvent
{
    private readonly int value;

    public IntPersistentStorageUpdateEvent(int value)
    {
        this.value = value;
    }

    public int Value => value;
}


public class StringPersistentStorageUpdateEvent : IPersistentStorageUpdateEvent
{
    private readonly string value;

    public StringPersistentStorageUpdateEvent(string value)
    {
        this.value = value;
    }

    public string Value => value;
}


// Interface to hide PlayerPrefs
public interface IPersistentDataStorage : IKeyObservable<IPersistentStorageUpdateEvent, string>
{
    int GetInt(string key);
    int GetInt(string key, int defaultValue);
    void SetInt(string key, int value);
    float GetFloat(string key);
    float GetFloat(string key, float defaultValue);
    void SetFloat(string key, float value);
    string GetString(string key);
    string GetString(string key, string defaultValue);
    void SetString(string key, string value);
    void ClearStorage();

}