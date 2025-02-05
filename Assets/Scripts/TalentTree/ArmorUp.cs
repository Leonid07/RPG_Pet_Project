using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmorUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] HealthUp _healthUp;
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] UIInsideCanvas _uIInsideCanvas;
    [SerializeField] int _priceLevel = 1;
    [SerializeField] TMP_Text _countPoints;
    public TMP_Text _levelCount;
    [SerializeField] Image _nextLevelImage;

    [SerializeField] GameObject _panelInfo;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] TMP_Text _discriptionText;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_healthUp._levelCount.text.Length >= 1)
            UpgradeArmor();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _headerText.text = "«ащита";
        _discriptionText.text = "ѕрокачка этого скила уменьшает воздействие физических атак на 1% за уровень";
        _panelInfo.SetActive(true);
        _panelInfo.transform.position = Input.mousePosition;
        _uIInsideCanvas.PanelInsideCanvas();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelInfo.SetActive(false);
    }
    private void UpgradeArmor()
    {
        switch (_levelCount.text)
        {
            case "":
                _nextLevelImage.color = Color.red;
                LevelUpArmor("I");
                break;
            case "I":
                LevelUpArmor("II");
                break;
            case "II":
                LevelUpArmor("III");
                break;
            case "III":
                LevelUpArmor("IV");
                break;
            case "IV":
                LevelUpArmor("V");
                break;
            case "V":
                LevelUpArmor("VI");
                break;
            case "VI":
                LevelUpArmor("VII");
                break;
            case "VII":
                LevelUpArmor("VIII");
                break;
            case "VIII":
                LevelUpArmor("IX");
                break;
            case "IX":
                LevelUpArmor("X");
                break;
        }
    }
    // ”величение ‘изической защиты персонажа на 1% за каждый уровень прокачки
    private void LevelUpArmor(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;
        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
        for (int count = 0; count <= 3; count++)
        {
            _playerCharacter.GetResistance()[count] += 1;
        }
    }
}
