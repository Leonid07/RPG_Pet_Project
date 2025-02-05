using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Radar;

public class RadarUpgrade : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Map map;
    [SerializeField] int[] _priceList = new int[2] { 2, 4 };
    string[] levelString = new string[2] { "I", "II" };
    [SerializeField] GameObject _radar;
    [SerializeField] TMP_Text _textLevel;
    [SerializeField] TMP_Text _countPoints;

    [Header("Для информационной панели")]
    [SerializeField] Button _buttonUpgrade;
    [SerializeField] GameObject _closePanelInfo;
    [SerializeField] GameObject _panelInfo;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] TMP_Text _discriptionText;

    [Header("Информация о таланте")]
    [SerializeField] TalentChange[] _talentChanges;
    private void Start()
    {
        _panelInfo.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_textLevel.text == levelString[levelString.Length - 1])
        {
            return;
        }
        else
        {
            _panelInfo.SetActive(true);
            _closePanelInfo.SetActive(true);
            _buttonUpgrade.onClick.AddListener(Upgrade);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_textLevel.text == levelString[levelString.Length - 1])
        {
            _panelInfo.SetActive(true);
            _headerText.text = "Улучшенный радар";
            _discriptionText.text = "Улучшенный радар показывает не только противников рядом но и полезные ресурсы не подалёку";
            _buttonUpgrade.gameObject.SetActive(false);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_textLevel.text == levelString[levelString.Length - 1])
        {
            _panelInfo.SetActive(false);
            _buttonUpgrade.gameObject.SetActive(true);
        }
    }
    //public void ClosePanelInfo()
    //{
    //    _panelInfo.SetActive(false);
    //    _closePanelInfo.SetActive(false);
    //}
    private void Upgrade()
    {
        switch (_textLevel.text)
        {
            case "":
                Changes(0);
                map.gameObject.SetActive(true);
                _headerText.text = _talentChanges[1].Header;
                _discriptionText.text = _talentChanges[1].Discription;
                map.UpdateMarkersPosition();
                break;
            case "I":
                Changes(1);
                map.ActiveStaticObject = true;
                _panelInfo.SetActive(false);
                _closePanelInfo.SetActive(false);
                map.UpdateMarkersPosition();
                break;
        }
    }
    private void Changes(int count)
    {
        if (_priceList[count] > Convert.ToInt32(_countPoints.text))
            return;

        int difference = Convert.ToInt32(_countPoints.text) - _priceList[count];
        _countPoints.text = difference.ToString();

        _textLevel.text = levelString[count];
    }

    [Serializable]
    public struct TalentChange
    {
        public string Header;
        public string Discription;
    }
}