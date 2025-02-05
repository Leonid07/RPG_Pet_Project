using System;
using TMPro;
using Player;
public class ProgressLevel
{
    //// ”величение максимального здоровь€ персонажа на 10 ед за каждый уровень прокачки
    //public static void LevelUpHealth(string level, TMP_Text _headerText, TMP_Text _discriptionText, TMP_Text _countPoints, int _priceLevel,
    //    TMP_Text _levelCount, PlayerCharacter _playerCharacter)
    //{
    //    if (_priceLevel > Convert.ToInt32(_countPoints.text))
    //        return;
    //    _levelCount.text = level;
    //    int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
    //    _countPoints.text = difference.ToString();
    //    _playerCharacter.SetMaxHealth(10);

    //    _headerText.text = "ћускулы";
    //    _discriptionText.text = "”величение максимального количества здоровь€ за каждый уровень здоровье повышаетс€ на 10 ед.";
    //}

    // ”величение ‘изической защиты персонажа на 1% за каждый уровень прокачки
//    public static void LevelUpArmor(string level, TMP_Text _headerText, TMP_Text _discriptionText, TMP_Text _countPoints, int _priceLevel,
//TMP_Text _levelCount, PlayerCharacter _playerCharacter)
//    {
//        if (_priceLevel > Convert.ToInt32(_countPoints.text))
//            return;
//        _levelCount.text = level;
//        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
//        _countPoints.text = difference.ToString();
//        for (int count = 0; count <= 3; count++)
//        {
//            _playerCharacter.GetResistance()[count] += 1;
//        }

//        _headerText.text = "«ащита";
//        _discriptionText.text = "ѕрокачка этого скила уменьшает воздействие физических атак на 1% за уровень";
//    }
    public static void LevelUpFistFight(string level, TMP_Text _headerText, TMP_Text _discriptionText, TMP_Text _countPoints, int _priceLevel,
TMP_Text _levelCount, PlayerCharacter _playerCharacter)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;

        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();
        _playerCharacter.SetDamage(_playerCharacter.GetDamage() + 10);

        _headerText.text = " улачный бой";
        _discriptionText.text = "”величени€ рукопашного урона на 10 ед.";
    }
    public static void LevelResistanceUp(string level, TMP_Text _headerText, TMP_Text _discriptionText, TMP_Text _countPoints, int _priceLevel,
TMP_Text _levelCount, PlayerCharacter _playerCharacter)
    {
        if (_priceLevel > Convert.ToInt32(_countPoints.text))
            return;

        _levelCount.text = level;
        int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
        _countPoints.text = difference.ToString();

        for (int count = 3; count <= _playerCharacter.GetResistance().Length; count++)
        {
            _playerCharacter.GetResistance()[count] += 1;
        }

        _headerText.text = " ристальна€ кожа";
        _discriptionText.text = "”величение всего магического сопративлени€ на 1% за каждую прокачку таланта";
    }
}
