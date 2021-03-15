using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Timers;

public class Counter : MonoBehaviour
{
    int counter = 0;
    public TMP_Text text;
    Timer timer;
    float offsetTime = 0;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        offsetTime = PlayerPrefs.HasKey("time") ? PlayerPrefs.GetFloat("time") : 0;
    }

    void Update()
    {
        float time = offsetTime + Time.unscaledTime;
        text.text = time.ToString();
        PlayerPrefs.SetFloat("time", time);
    }
}
