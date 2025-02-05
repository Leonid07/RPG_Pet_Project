using Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManaUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] WeaponDamageUp _weaponDamageUp;
    [SerializeField] ResistanceUp _resistanceUp;
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] UIInsideCanvas _uIInsideCanvas;
    [SerializeField] int _priceLevel = 1;
    [HideInInspector] public int DamageMultiplier = 0;
    [SerializeField] TMP_Text _countPoints;
    [SerializeField] TMP_Text _levelCount;

    [SerializeField] GameObject _panelInfo;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] TMP_Text _discriptionText;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_weaponDamageUp._levelCount.text.Length >= 1 && _resistanceUp._levelCount.text.Length >= 1)
            UpgradeUpEnergy();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _headerText.text = "Глубокое познание";
        _discriptionText.text = "Увеличение маны на 25 ед. за каждый лвл";
        _panelInfo.SetActive(true);
        _panelInfo.transform.position = Input.mousePosition;
        _uIInsideCanvas.PanelInsideCanvas();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelInfo.SetActive(false);
    }
    private void UpgradeUpEnergy()
    {
        switch (_levelCount.text)
        {
            case "":
                LevelUpEnergy("I");
                break;
            case "I":
                LevelUpEnergy("II");
                break;
            case "II":
                LevelUpEnergy("III");
                break;
            case "III":
                LevelUpEnergy("IV");
                break;
        }
    }
    public void LevelUpEnergy(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;
        _playerCharacter.SetMaxEnergy(25);
        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
    }
}
