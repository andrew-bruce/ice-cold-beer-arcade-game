using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderHandleScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Outline masterOutline;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Outline>().enabled = true;
        if (masterOutline != null)
        {
            masterOutline.enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Outline>().enabled = false;
    }
}
