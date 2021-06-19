using System;
using Logging;
using UnityEngine;

[RequireComponent(typeof(PoppingTextSpawnerComponent))]
public class WalletTextComponent : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text expText;
    [SerializeField]
    TMPro.TMP_Text goldText;

    IObserver<IPlayerWalletUpdateEvent> observer;

    PoppingTextSpawnerComponent textPopper;

    static readonly LilLogger logger = new LilLogger(typeof(WalletTextComponent).ToString());
    double currentExp;
    double currentGold;
    private void Start()
    {
        observer = new SimpleObserver<IPlayerWalletUpdateEvent>(SingletonProvider.MainPlayerWallet, e => UpdateText());
        UpdateText();
        textPopper = GetComponent<PoppingTextSpawnerComponent>();
    }

    private void UpdateExp()
    {
        var oldExp = currentExp;
        currentExp = SingletonProvider.MainPlayerWallet.Experience;
        expText.text = Mathf.RoundToInt((float)currentExp) + " Exp";
        if (currentExp != oldExp && textPopper)
        {
            textPopper.SpawnText(Mathf.RoundToInt((float)(currentExp - oldExp)).ToString());
        }

    }

    private void UpdateGold()
    {
        var oldGold = currentGold;
        currentGold = SingletonProvider.MainPlayerWallet.Gold;
        goldText.text = Mathf.RoundToInt((float)currentGold) + " Gold";
        if (currentGold != oldGold && textPopper)
        {
            textPopper.SpawnText(Mathf.RoundToInt((float)(currentGold - oldGold)).ToString());
        }

    }
    private void UpdateText()
    {
        UpdateGold();
        UpdateExp();
    }
}