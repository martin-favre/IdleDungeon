using System.Collections.Generic;
using UnityEngine;

public static class EnemyTemplates
{
    static readonly EnemyTemplate ratTemplate = new EnemyTemplate(
        SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/RatMat"),
        "Rat",
        spriteScale: new Vector2(0.5f, 0.5f),
        offset: new Vector2(0, -3),
        maxHp: 15,
        attack: 1,
        speed: 1.2f,
        worthFactor: 0.7f,
        new ActionFactory(() => new[] { new AttackRandomAction("Sprites/addon_04", "Attack") })
        );

    static readonly EnemyTemplate mudGolemTemplate = new EnemyTemplate(
        SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/MudGolemMat"),
        "Mud Golem",
        spriteScale: new Vector2(0.7f, 0.7f),
        offset: new Vector2(0, 0),
        maxHp: 40,
        attack: 2,
        speed: 0.5f,
        worthFactor: 1.5f,
        new ActionFactory(() => new[] { new AttackRandomAction("Sprites/addon_04", "Attack") })
        );

    static readonly EnemyTemplate slimeTemplate = new EnemyTemplate(
        SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/SlimeMat"),
        "Slime",
        spriteScale: new Vector2(0.5f, 0.5f),
        offset: new Vector2(0, -3),
        maxHp: 30,
        attack: 1,
        speed: 1f,
        worthFactor: 1.5f,
        new ActionFactory(() => new[] { new AttackRandomAction("Sprites/addon_04", "Attack") })
        );
    static readonly EnemyTemplate eyeBatTemplate = new EnemyTemplate(
        SingletonProvider.MainGameobjectLoader.GetPrefab<Material>("Materials/EyeBatMat"),
        "Eyebat",
        spriteScale: new Vector2(0.5f, 0.5f),
        offset: new Vector2(0, 0),
        maxHp: 20,
        attack: 1.5f,
        speed: 1.1f,
        worthFactor: 1f,
        new ActionFactory(() => new[] { new AttackRandomAction("Sprites/addon_04", "Attack") })
        );

    public static List<ICharacter> GetRandomEncounter()
    {
        return new List<ICharacter>() { new EnemyCharacter(ratTemplate) };
    }
}


