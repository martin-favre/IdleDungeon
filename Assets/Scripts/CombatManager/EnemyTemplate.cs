using UnityEngine;

public class EnemyTemplate
{
    private readonly Material material;
    private readonly string name;

    public EnemyTemplate(Material material, string name)
    {
        this.material = material;
        this.name = name;
    }

    public Material Material => material;

    public string Name => name;
}