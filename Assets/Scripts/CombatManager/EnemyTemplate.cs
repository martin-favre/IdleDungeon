using System;
using UnityEngine;
public class ActionFactory
{
    private readonly Func<ICharacterAction[]> foo;

    public ActionFactory(Func<ICharacterAction[]> foo)
    {
        this.foo = foo;
    }

    public ICharacterAction[] SpawnActions() { return foo(); }
}
public class EnemyTemplate
{
    private readonly Material material;
    private readonly string name;
    private readonly Vector2 spriteScale;
    private readonly Vector3 offset;
    private readonly int maxHp;
    private readonly float attack;
    private readonly float speed;
    private readonly float worthFactor;
    private readonly ActionFactory factory;

    public EnemyTemplate(Material material, string name, Vector2 spriteScale, Vector3 offset, int maxHp, float attack, float speed, float worthFactor,
    ActionFactory factory)
    {
        this.material = material;
        this.name = name;
        this.spriteScale = spriteScale;
        this.offset = offset;
        this.maxHp = maxHp;
        this.attack = attack;
        this.speed = speed;
        this.worthFactor = worthFactor;
        this.factory = factory;
    }

    public Material Material => material;

    public string Name => name;

    public Vector2 SpriteScale => spriteScale;

    public Vector3 Offset => offset;

    public int MaxHp => maxHp;

    public float Attack => attack;

    public float Speed => speed;

    public float WorthFactor => worthFactor;

    public ActionFactory Factory => factory;
}