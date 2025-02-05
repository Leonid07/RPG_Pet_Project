using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using System;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;

    [SerializeField] private Button _healthPlusButton;
    [SerializeField] private Button _healthMinusButton;

    [SerializeField] private Button _energyPlusButton;
    [SerializeField] private Button _energyMinusButton;

    [SerializeField] private Button _characterButtonPlus;
    [SerializeField] private Button _characterButtonMinus;
    public float valueCharacter = 10;
    private void Awake()
    {
        _healthPlusButton.onClick.AddListener(HealPlus);
        _healthMinusButton.onClick.AddListener(HealthMinus);

        _energyPlusButton.onClick.AddListener(EnergyPlus);
        _energyMinusButton.onClick.AddListener(EnergyMinus);

        _characterButtonPlus.onClick.AddListener(delegate { playerCharacter.UpValueResistance(valueCharacter); });
        _characterButtonMinus.onClick.AddListener(delegate { playerCharacter.DownValueResistance(valueCharacter); });
    }

    public void HealthMinus()
    {
        if (playerCharacter.GetHealth() > 0)
        {
            playerCharacter.SetMaxHealth(-10);
            playerCharacter.SetHealthPlus(-10);
        }
    }
    public void HealPlus()
    {
        playerCharacter.SetMaxHealth(10);
        playerCharacter.SetHealthPlus(10);
    }
    public void EnergyPlus()
    {
        playerCharacter.SetMaxEnergy(10);
        playerCharacter.SetEnergyPlus(10);
    }
    public void EnergyMinus()
    {
        if (playerCharacter.GetEnergy() > 0)
        {
            playerCharacter.SetMaxEnergy(-10);
            playerCharacter.SetEnergyPlus(-10);
        }
    }
}