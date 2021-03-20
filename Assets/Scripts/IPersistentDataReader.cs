
public interface IPersistentDataReader {
    int GetInt(string key);    
    int GetInt(string key, int defaultValue);    
    float GetFloat(string key);
    float GetFloat(string key, float defaultValue);
    string GetString(string key);    
    string GetString(string key, string defaultValue);    
}