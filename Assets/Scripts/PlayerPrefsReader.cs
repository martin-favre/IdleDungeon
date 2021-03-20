using UnityEngine;

public class PlayerPrefsReader : IPersistentDataReader
{
    private static PlayerPrefsReader instance;
    static PlayerPrefsReader() {
        instance = new PlayerPrefsReader();
        PlayerPrefs.DeleteAll(); // clear all during development
    }

    public static PlayerPrefsReader Instance { get => instance; set => instance = value; }

    public float GetFloat(string key) {
        return GetFloat(key, 0);
    }

    public float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key); 
    }

    public int GetInt(string key, int defaultValue)
    {
        return GetInt(key, 0);
    }

    public string GetString(string key)
    {
        return GetString(key, "");
    }

    public string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

}