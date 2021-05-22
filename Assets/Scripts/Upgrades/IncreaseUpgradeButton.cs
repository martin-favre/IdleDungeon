using UnityEngine;
using TMPro;
using Logging;

public class IncreaseUpgradeButton : MonoBehaviour
{

    [SerializeField]
    private string upgradeKey;
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
        
    }

    public void SetButtonType(int playerIndex, int upgradeLevel, UpgradeTabContentComponent.UpgradeType type)
    {
        switch (type)
        {
            case UpgradeTabContentComponent.UpgradeType.Attackiness:
                upgradeKey = PlayerAttributes.GetAttackinessUpgradeKey(upgradeLevel, playerIndex);
                break;
            case UpgradeTabContentComponent.UpgradeType.Healthiness:
                upgradeKey = PlayerAttributes.GetHealthinessUpgradeKey(upgradeLevel, playerIndex);
                break;
            default:
                throw new System.Exception("Unknown Upgradetype " + type);
        }
        observer = new KeyObserver<string, Upgrade>(UpgradeManager.Instance, upgradeKey, UpdateText);
        UpdateText(UpgradeManager.Instance.GetUpgrade(upgradeKey));
    }

    public void LevelUp()
    {
        UpgradeManager.Instance.LevelUpUpgrade(upgradeKey);
    }

    void UpdateText(Upgrade upgrade)
    {
        if(upgrade == null) return;
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
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeKey);
        if (upgrade != null)
        {
            UpdateText(upgrade);
        }
    }
}