using Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WeaponDamageUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] FistFightUp _fistFightUp;
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] UIInsideCanvas _uIInsideCanvas;
    [SerializeField] int _priceLevel = 1;
    [HideInInspector] public int DamageMultiplier = 0;
    [SerializeField] TMP_Text _countPoints;
    public TMP_Text _levelCount;
    [SerializeField] Image _nextLevelImage;

    [SerializeField] GameObject _panelInfo;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] TMP_Text _discriptionText;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_fistFightUp._levelCount.text.Length >= 1)
            UpgradeWeaponDamage();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _headerText.text = "Мастер оружия";
        _discriptionText.text = "Увеличение всего урона от оружия на 1% за каждый лвл";
        _panelInfo.SetActive(true);
        _panelInfo.transform.position = Input.mousePosition;
        _uIInsideCanvas.PanelInsideCanvas();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelInfo.SetActive(false);
    }
    private void UpgradeWeaponDamage()
    {
        switch (_levelCount.text)
        {
            case "":
                _nextLevelImage.color = Color.red;
                LevelWeaponDamage("I");
                break;
            case "I":
                LevelWeaponDamage("II");
                break;
            case "II":
                LevelWeaponDamage("III");
                break;
            case "III":
                LevelWeaponDamage("IV");
                break;
        }
    }
    public void LevelWeaponDamage(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;
        DamageMultiplier++;
        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
    }
}
