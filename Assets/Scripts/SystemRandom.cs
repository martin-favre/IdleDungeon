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
}