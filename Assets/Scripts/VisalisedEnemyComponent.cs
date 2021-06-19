using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisalisedEnemyComponent : MonoBehaviour
{

    [SerializeField]
    EnemyTargetComponent enemyTargetComponent;
    ICharacter character;
    public ICharacter Character { get => character; }

    public void SetCharacter(ICharacter character)
    {
        this.character = character;
        if (enemyTargetComponent)
        {
            enemyTargetComponent.SetCharacter(character);
        }
    }
}
