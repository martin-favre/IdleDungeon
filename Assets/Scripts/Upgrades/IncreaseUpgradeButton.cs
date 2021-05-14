using UnityEngine;
using TMPro;
using Logging;

public class IncreaseUpgradeButton : MonoBehaviour
{

    [SerializeField]
    private string upgradeType;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text costText;

    private static readonly LilLogger logger = new LilLogger(typeof(IncreaseUpgradeButton).ToString());
    KeyObserver<string, Upgrade> observer;
    void Awake()
    {
        if (levelText == null) logger.Log("Missing field " + nameof(levelText), LogLevel.Warning);
        if (costText == null) logger.Log("Missing field " + nameof(costText), LogLevel.Warning);
    }
    void Start()
    {
        observer = new KeyObserver<string, Upgrade>(UpgradeManager.Instance, upgradeType, UpdateText);
    }

    public void LevelUp()
    {
        UpgradeManager.Instance.LevelUpUpgrade(upgradeType);
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

    private void OnEnable()
    {
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeType);
        if(upgrade!= null) {
            UpdateText(upgrade);
        }
    }
}