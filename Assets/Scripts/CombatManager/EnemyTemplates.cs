using UnityEngine;

public static class EnemyTemplates
{
    static readonly EnemyTemplate[] templates =
    {
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/RatMat"), "Rat", new Vector2(0.5f, 0.5f), new Vector2(0, -3)),
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/SlimeMat"), "Slime", new Vector2(0.5f, 0.5f), new Vector2(0, -3)),
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/MudGolemMat"), "Mud Golem", new Vector2(0.7f, 0.7f), new Vector2(0, 0)),
        new EnemyTemplate(SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/EyeBatMat"), "Eyebat", new Vector2(0.5f, 0.5f), new Vector2(0, 0))
    };

    public static EnemyTemplate GetRandom()
    {
        var index = SingletonProvider.MainRandomProvider.RandomInt(0, templates.Length);
        return templates[index];
    }
}