using System;
using Logging;
using UnityEngine;

[RequireComponent(typeof(PoppingTextSpawnerComponent))]
public class WalletTextComponent : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text expText;

    IObserver<IPlayerWalletUpdateEvent> observer;

    PoppingTextSpawnerComponent textPopper;

    static readonly LilLogger logger = new LilLogger(typeof(WalletTextComponent).ToString());
    double currentExp;
    private void Start()
    {
        observer = new SimpleObserver<IPlayerWalletUpdateEvent>(SingletonProvider.MainPlayerWallet, e => UpdateText());
        UpdateText();
        textPopper = GetComponent<PoppingTextSpawnerComponent>();
    }

    private void UpdateText()
    {
        var oldExp = currentExp;
        currentExp = SingletonProvider.MainPlayerWallet.Experience;
        expText.text = Mathf.RoundToInt((float)currentExp) + " Exp";
        if (currentExp != oldExp && textPopper)
        {
            textPopper.SpawnText(Mathf.RoundToInt((float)(currentExp-oldExp)).ToString());
        }
    }
}