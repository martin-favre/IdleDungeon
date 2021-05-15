using System;

public class SystemGuid : IGuid
{
    private Guid guid = Guid.NewGuid();
    public bool Equals(IGuid other)
    {
        var o = other as SystemGuid;
        if (o == null) return false;
        return o.guid.Equals(guid);
    }
}