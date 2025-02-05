using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FistFightUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
            UpgradeFistFight();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _headerText.text = "Кулачный бой";
        _discriptionText.text = "Увеличения рукопашного урона на 10 ед.";
        _panelInfo.SetActive(true);
        _panelInfo.transform.position = Input.mousePosition;
        _uIInsideCanvas.PanelInsideCanvas();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelInfo.SetActive(false);
    }
    private void UpgradeFistFight()
    {
        switch (_levelCount.text)
        {
            case "":
                _nextLevelImage.color = Color.red;
                LevelUpFistFight("I");
                break;
        }
    }
    // Увеличения рукопашного урона на 10 ед.
    public void LevelUpFistFight(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;

        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
        _playerCharacter.SetDamage(_playerCharacter.GetDamage() + 10);
    }
}