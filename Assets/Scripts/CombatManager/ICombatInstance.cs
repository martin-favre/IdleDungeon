using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(List<ICombatant> playerChars);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    public ICombatInstance CreateInstance(List<ICombatant> playerChars)
    {
        return new CombatInstance(playerChars);
    }
}

public interface ICombatInstance
{
    bool IsDone();
    void Update();
}
