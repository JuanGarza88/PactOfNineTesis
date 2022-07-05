using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData ped)
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.MenuSelect);
    }

    public void OnPointerExit(PointerEventData ped)
    {
        SFXManager.Instance.PlaySFX(SFXManager.SFXName.MenuSelect);
    }
}
