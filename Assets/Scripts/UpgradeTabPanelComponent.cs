using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeTabPanelComponent : MonoBehaviour
{

    public enum SelectedTab
    {
        Attack,
        Health
    }
    [SerializeField]
    private GameObject attackButtonGobj;
    [SerializeField]
    private GameObject healthButtonGobj;
    [SerializeField]
    private GameObject attackPanelGobj;
    [SerializeField]
    private GameObject healthPanelGobj;
    [SerializeField]
    private Sprite activeColor;
    [SerializeField]
    private Sprite inactiveColor;

    private readonly static LilLogger logger = new LilLogger(typeof(UpgradeTabPanelComponent).ToString());

    private SelectedTab selectedTab;

    Dictionary<SelectedTab, GameObject> tabRef = new Dictionary<SelectedTab, GameObject>();

    void Start()
    {
        if (attackButtonGobj == null) logger.Log(nameof(attackButtonGobj) + " is null", LogLevel.Warning);
        if (healthButtonGobj == null) logger.Log(nameof(healthButtonGobj) + " is null", LogLevel.Warning);
        if (attackPanelGobj == null) logger.Log(nameof(attackPanelGobj) + " is null", LogLevel.Warning);
        if (healthPanelGobj == null) logger.Log(nameof(healthPanelGobj) + " is null", LogLevel.Warning);
        tabRef[SelectedTab.Attack] = attackButtonGobj;
        tabRef[SelectedTab.Health] = healthButtonGobj;
        selectedTab = SelectedTab.Health; // just so it doesn't get ignored when we initially "switch" to attack
        OnTabButtonPressed(SelectedTab.Attack);
    }

    public void OnAttackButtonPressed()
    {
        OnTabButtonPressed(SelectedTab.Attack);
    }

    public void OnHealthButtonPressed()
    {
        OnTabButtonPressed(SelectedTab.Health);
    }


    private void OnTabButtonPressed(SelectedTab tab)
    {
        if (tab == selectedTab) return;
        if (!tabRef.ContainsKey(tab))
        {
            logger.Log("Unknown tabtype " + tab.ToString(), LogLevel.Warning);
            return;
        }
        selectedTab = tab;
        SetTabActive(selectedTab);
        SetAllOtherTabsInactive(selectedTab);
    }

    private void SetColorOnTab(SelectedTab selected, Sprite color)
    {
        GameObject tab;
        tabRef.TryGetValue(selected, out tab);
        if (tab)
        {
            var img = tab.GetComponent<Image>();
            if (!img) return;
            img.sprite = color;
        }

    }

    private void SetTabActive(SelectedTab selected)
    {
        SetColorOnTab(selected, activeColor);
    }

    private void SetTabInactive(SelectedTab selected)
    {
        SetColorOnTab(selected, inactiveColor);
    }

    private void SetAllOtherTabsInactive(SelectedTab selected)
    {
        foreach (var tab in tabRef.Keys)
        {
            if (tab != selected) SetTabInactive(tab);
        }
    }
}
