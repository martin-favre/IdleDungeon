using System;

public interface ICombatAttributes: IDisposable
{
    double Attack { get; }
    double Speed { get; }
}
