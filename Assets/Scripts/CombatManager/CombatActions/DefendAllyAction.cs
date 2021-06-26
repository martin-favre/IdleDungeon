
using System;
using UnityEngine;

public class DefendAllyAction : BaseCharacterAction
{

    public DefendAllyAction(string iconPath, float baseActionTime) : base(iconPath, baseActionTime, null, false, true)
    {
    }

    public override void PerformAction(ICharacter user, ICombatReader combat)
    {
        if (Target != null && !Target.IsDead())
        {
            Target.ApplyBuff(new DefendBuff(new WeakReference<ICharacter>(user)));
        }
    }
}
