using UnityEngine;

public class PlayerWalletDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<IPlayerWalletUpdateEvent> observer;

    void Start()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        UpdateText(SingletonProvider.MainPlayerWallet);
        observer = new SimpleObserver<IPlayerWalletUpdateEvent>(SingletonProvider.MainPlayerWallet, (e) => UpdateText(e.Wallet));
    }

    private void UpdateText(IPlayerWallet wallet)
    {
        if(wallet == null) return;
        displayer.UpdateValue("Experience: " + Mathf.RoundToInt((float)wallet.Experience));
    }
}