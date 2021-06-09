using UnityEngine;

public class EnemyTemplate
{
    private readonly Material material;
    private readonly string name;
    private readonly Vector2 spriteScale;

    public EnemyTemplate(Material material, string name, Vector2 spriteScale)
    {
        this.material = material;
        this.name = name;
        this.spriteScale = spriteScale;
    }

    public Material Material => material;

    public string Name => name;

    public Vector2 SpriteScale => spriteScale;
}