
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuffIconComponent : MonoBehaviour
{
    Image icon;
    void Awake()
    {
        icon = GetComponent<Image>();
    }
    public void SetBuff(IBuff buff)
    {
        if (buff != null)
        {
            icon.sprite = buff.Icon;
        }
    }
}