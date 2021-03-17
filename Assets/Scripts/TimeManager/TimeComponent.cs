using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to scale time up and down
public class TimeComponent : MonoBehaviour
{
    [SerializeField] 
    private float timeScaling = 1;

    void Update() {
        
        UnityTime.Instance.TimeScaling = timeScaling;
    }
}
