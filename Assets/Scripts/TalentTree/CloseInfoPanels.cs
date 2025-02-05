using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseInfoPanels : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject[] _panelInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject panel in _panelInfo)
        {
            panel.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
