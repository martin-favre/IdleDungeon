public class SystemRandom : IRandomProvider
{
    private static SystemRandom instance;

    public static SystemRandom Instance { get => instance; set => instance = value; }

    System.Random random;

    static SystemRandom()
    {
        instance = new SystemRandom();
    }

    private SystemRandom()
    {
        random = new System.Random();
    }

    public int RandomInt(int min, int max)
    {
        return random.Next(min, max);
    }

    public bool ThingHappens(float chance)
    {
        return random.NextDouble() <= chance;
    }

    public float RandomFloat(float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}