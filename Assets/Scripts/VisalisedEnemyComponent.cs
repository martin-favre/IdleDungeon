using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisalisedEnemyComponent : MonoBehaviour
{
    ICharacter character;

    public ICharacter Character { get => character; }

    public void SetCharacter(ICharacter character)
    {
        this.character = character;
    }
}
