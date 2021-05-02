using UnityEngine;

public class PlayerWalletDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<IPlayerWalletUpdateEvent> observer;

    void Awake()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        UpdateText(PlayerWallet.Instance);
        observer = new SimpleObserver<IPlayerWalletUpdateEvent>(PlayerWallet.Instance, (e) => UpdateText(e.Wallet));
    }

    private void UpdateText(IPlayerWallet wallet)
    {
        if(wallet == null) return;
        displayer.UpdateValue("Experience: " + Mathf.RoundToInt((float)wallet.Experience));
    }
}