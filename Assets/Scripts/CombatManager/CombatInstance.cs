
public class CombatResults
{

}

public class CombatInstance
{

    float startTime;

    public CombatInstance()
    {
        startTime = UnityTime.Instance.Time;
    }
    public void Update()
    {
    }

    public bool IsDone()
    {
        return UnityTime.Instance.Time > startTime + 5f;
    }
}