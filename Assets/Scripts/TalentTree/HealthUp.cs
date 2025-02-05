using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HealthUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] UIInsideCanvas _uIInsideCanvas;
    [SerializeField] int _priceLevel = 1;
    [SerializeField] TMP_Text _countPoints;
    public TMP_Text _levelCount;
    [SerializeField] Image _nextLevelImage, _nextLevelImage2;

    [SerializeField] GameObject _panelInfo;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] TMP_Text _discriptionText;
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeHealth();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _headerText.text = "Мускулы";
        _discriptionText.text = "Увеличение максимального количества здоровья за каждый уровень здоровье повышается на 10 ед.";
        _panelInfo.SetActive(true);
        _panelInfo.transform.position = Input.mousePosition;
        _uIInsideCanvas.PanelInsideCanvas();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelInfo.SetActive(false);
    }
    private void UpgradeHealth()
    {
        switch (_levelCount.text)
        {
            case "":
                _nextLevelImage.color = Color.red;
                _nextLevelImage2.color = Color.red;
                LevelUpHealth("I");
                break;
            case "I":
                LevelUpHealth("II");
                break;
            case "II":
                LevelUpHealth("III");
                break;
            case "III":
                LevelUpHealth("IV");
                break;
            case "IV":
                LevelUpHealth("V");
                break;
            case "V":
                LevelUpHealth("VI");
                break;
            case "VI":
                LevelUpHealth("VII");
                break;
            case "VII":
                LevelUpHealth("VIII");
                break;
            case "VIII":
                LevelUpHealth("IX");
                break;
            case "IX":
                LevelUpHealth("X");
                break;
        }
    }
    // Увеличение максимального здоровья персонажа на 10 ед за каждый уровень прокачки
    private void LevelUpHealth(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;
        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
        _playerCharacter.SetMaxHealth(10);
    }
}
