
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class UITurnProgressIndicator : MonoBehaviour
{
    [SerializeField]
    float updateFrequencyS = 0.01f;
    [SerializeField]
    UIBarComponent bar;
    [SerializeField]
    Image background;

    private IHasTurnProgress hasProgress;

    float startTime;

    void Start()
    {
        startTime = SingletonProvider.MainTimeProvider.Time;
        UpdateBarFill();
    }

    public void SetTurnProgressOwner(IHasTurnProgress hasProgress)
    {
        this.hasProgress = hasProgress;
        UpdateBarFill();
    }

    private void UpdateBarFill()
    {
        if (hasProgress != null && hasProgress.TurnProgress != null)
        {
            if (bar)
            {
                bar.gameObject.SetActive(true);
                bar.SetFill(hasProgress.TurnProgress.GetRelativeProgress());
            }
            if (background) background.gameObject.SetActive(true);

        }
        else
        {
            if (bar) bar.gameObject.SetActive(false);
            if (background) background.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (startTime + updateFrequencyS < SingletonProvider.MainTimeProvider.Time)
        {
            UpdateBarFill();
            startTime = SingletonProvider.MainTimeProvider.Time;
        }
    }
}
