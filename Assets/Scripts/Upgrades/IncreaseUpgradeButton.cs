using UnityEngine;
using TMPro;
using Logging;

public class IncreaseUpgradeButton : MonoBehaviour
{

    [SerializeField]
    private UpgradeType upgradeType;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text costText;

    private static readonly LilLogger logger = new LilLogger(typeof(IncreaseUpgradeButton).ToString());
    SimpleObserver<Upgrade> observer;
    void Awake()
    {
        if (levelText == null) logger.Log("Missing field " + nameof(levelText), LogLevel.Warning);
        if (costText == null) logger.Log("Missing field " + nameof(costText), LogLevel.Warning);
    }
    void Start()
    {
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeType);
        if (upgrade != null)
        {
            observer = new SimpleObserver<Upgrade>(upgrade, UpdateText);
            UpdateText(upgrade);
        }
    }

    public void LevelUp()
    {
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeType);
        if (upgrade != null)
        {
            upgrade.LevelUp();
        }
    }

    void UpdateText(Upgrade upgrade)
    {
        if (levelText)
        {
            levelText.text = "Level:" + upgrade.Level;
        }
        if (costText)
        {
            costText.text = "Cost:" + Mathf.CeilToInt(upgrade.Cost);
        }
    }
}