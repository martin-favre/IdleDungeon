
using System.Timers;
using UnityEngine;

public class UITurnProgressIndicator : MonoBehaviour
{
    [SerializeField]
    float updateFrequencyS = 0.5f;
    [SerializeField]
    UIBarComponent bar;
    private ICharacter character;

    float startTime;

    void Start()
    {
        startTime = SingletonProvider.MainTimeProvider.Time;
    }

    public void SetCharacter(ICharacter character)
    {
        this.character = character;
    }

    private void Update()
    {
        if (bar && character != null && character.TurnProgress != null)
        {
            if (startTime + updateFrequencyS < SingletonProvider.MainTimeProvider.Time)
            {
                bar.SetFill(character.TurnProgress.GetRelativeProgress());
            }
        }
    }
}
