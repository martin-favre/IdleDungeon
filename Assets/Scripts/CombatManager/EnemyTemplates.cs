using UnityEngine;

public static class EnemyTemplates
{
    static readonly EnemyTemplate[] templates =
    {
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/RatMat"), "Rat"),
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/SlimeMat"), "Slime")
    };

    public static EnemyTemplate GetRandom()
    {
        var index = SingletonProvider.MainRandomProvider.RandomInt(0, templates.Length);
        return templates[index];
    }
}