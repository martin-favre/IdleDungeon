using System;
using UnityEngine;

public class WalletTextComponent : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text expText;

    IObserver<IPlayerWalletUpdateEvent> observer;

    private void Start()
    {
        observer = new SimpleObserver<IPlayerWalletUpdateEvent>(SingletonProvider.MainPlayerWallet, e => UpdateText());
        UpdateText();
    }

    private void UpdateText()
    {
        expText.text = Mathf.RoundToInt((float)SingletonProvider.MainPlayerWallet.Experience) + " Exp";
    }
}