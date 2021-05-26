using UnityEngine;

public class UpgradePanelComponent : MonoBehaviour
{
    [SerializeField]
    private UpgradeTabContentComponent[] tabComponents;

    private static UpgradePanelComponent instance;

    public static UpgradePanelComponent Instance { get => instance; }

    void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPanel(int index)
    {
        foreach (var tab in tabComponents)
        {
            tab.SetPlayerIndex(index);
        }
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}