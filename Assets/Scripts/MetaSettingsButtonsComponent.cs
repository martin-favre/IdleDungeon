using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaSettingsButtonsComponent : MonoBehaviour
{
    public void ClearDB()
    {
        SingletonProvider.MainDataStorage.ClearStorage();
    }

    public void Give1KExp()
    {
        SingletonProvider.MainPlayerWallet.AddExperience(1000);
    }
}
