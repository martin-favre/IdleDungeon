using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : ICombatAttributes
{
    private readonly float attack;
    private readonly float speed;

    public PlayerAttributes(float attack, float speed)
    {
        this.attack = attack;
        this.speed = speed;
    }

    public float Attack => attack;

    public float Speed => speed;
}