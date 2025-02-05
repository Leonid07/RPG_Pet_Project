using Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResistanceUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] ArmorUp _armorUp;
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
        if (_armorUp._levelCount.text.Length >= 1)
            UpgradeHealth();
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
    private void UpgradeHealth()
    {
        switch (_levelCount.text)
        {
            case "":
                _nextLevelImage.color = Color.red;
                LevelResistanceUp("I");
                break;
            case "I":
                LevelResistanceUp("II");
                break;
            case "II":
                LevelResistanceUp("III");
                break;
            case "III":
                LevelResistanceUp("IV");
                break;
            case "IV":
                LevelResistanceUp("V");
                break;
        }
    }
    public void LevelResistanceUp(string level)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;

        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();

        for (int count = 4; count < _playerCharacter.GetResistance().Length; count++)
        {
            _playerCharacter.GetResistance()[count] += 1;
        }
    }
}
