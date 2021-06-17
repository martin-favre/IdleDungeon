using UnityEngine;

public class EnemyTemplate
{
    private readonly Material material;
    private readonly string name;
    private readonly Vector2 spriteScale;
    private readonly Vector3 offset;

    public EnemyTemplate(Material material, string name, Vector2 spriteScale, Vector3 offset)
    {
        this.material = material;
        this.name = name;
        this.spriteScale = spriteScale;
        this.offset = offset;
    }

    public Material Material => material;

    public string Name => name;

    public Vector2 SpriteScale => spriteScale;

    public Vector3 Offset => offset;
}