public class GuidProvider : IGuidProvider
{
    static IGuidProvider instance;

    public static IGuidProvider Instance { get => instance; }

    static GuidProvider()
    {
        instance = new GuidProvider();
    }

    public IGuid GetNewGuid()
    {
        return new SystemGuid();
    }
}