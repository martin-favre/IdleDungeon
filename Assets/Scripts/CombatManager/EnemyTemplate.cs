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
    private readonly double maxHp;
    private readonly double attack;
    private readonly double speed;
    private readonly double worthFactor;
    private readonly ActionFactory factory;

    public EnemyTemplate(Material material, string name, Vector2 spriteScale, Vector3 offset, double maxHp, double attack, double speed, double worthFactor,
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

    public double MaxHp => maxHp;

    public double Attack => attack;

    public double Speed => speed;

    public double WorthFactor => worthFactor;

    public ActionFactory Factory => factory;
}