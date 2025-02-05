using System;
using TMPro;
using Player;
public class ProgressLevel
{
    //// ���������� ������������� �������� ��������� �� 10 �� �� ������ ������� ��������
    //public static void LevelUpHealth(string level, TMP_Text _headerText, TMP_Text _discriptionText, TMP_Text _countPoints, int _priceLevel,
    //    TMP_Text _levelCount, PlayerCharacter _playerCharacter)
    //{
    //    if (_priceLevel > Convert.ToInt32(_countPoints.text))
    //        return;
    //    _levelCount.text = level;
    //    int difference = Convert.ToInt32(_countPoints.text) - _priceLevel;
    //    _countPoints.text = difference.ToString();
    //    _playerCharacter.SetMaxHealth(10);

    //    _headerText.text = "�������";
    //    _discriptionText.text = "���������� ������������� ���������� �������� �� ������ ������� �������� ���������� �� 10 ��.";
    //}

    // ���������� ���������� ������ ��������� �� 1% �� ������ ������� ��������
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

//        _headerText.text = "������";
//        _discriptionText.text = "�������� ����� ����� ��������� ����������� ���������� ���� �� 1% �� �������";
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

        _headerText.text = "�������� ���";
        _discriptionText.text = "���������� ����������� ����� �� 10 ��.";
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

        _headerText.text = "����������� ����";
        _discriptionText.text = "���������� ����� ����������� ������������� �� 1% �� ������ �������� �������";
    }
}
