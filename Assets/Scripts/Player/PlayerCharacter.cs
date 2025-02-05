using Inventory;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Показатели характеристик")]
        [SerializeField] private float _health;//здоровье
        [SerializeField] private float _energy;//энергия
        [SerializeField] float _damageFromHand = 5f;

        [SerializeField] float _countRegenHealth;
        [SerializeField] float _timeHealthRegen;
        [SerializeField] float _countRegenEnergy;
        [SerializeField] float _timeEnergyRegen;

        [Header("Сопротивления")]
        [SerializeField] private float[] Resistance = new float[16];
        [SerializeField] private Text _textHealth;
        [SerializeField] private Text _textEnergy;
        #region
        // 0 //_pricking;//колющий
        // 1 //_cutting;//режущий
        // 2 //_crushing;//дробящий
        // 3 //_chopping;//рубящий
        // 4 //_fireResistance;//огонь
        // 5 //_poisonResistance;//яд
        // 6 //_iceResistance;//лёд
        // 7 //_lightResistance;//свет
        // 8 //_darknessResistance;//тьма
        // 9 //_electricityResistance;//электрошок
        // 10 //_groundResistance;//земля
        // 11 //_airResistance;//воздух
        // 12 //_bleedingResistance;//кровотечение
        // 13 //_curceResistance;// проклятие
        // 14 //_diseaseResistance;// болезни
        // 15 //_controlEffectsResistance;// длительность контроля
        #endregion
        [Space(20)]
        //слайдеры нижнего левого края
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _enSlider;

        // харектеристики в правой панели
        [SerializeField] private Text _hpText;
        [SerializeField] private Text _enText;
        public GameObject panelAtribute;

        //панель характеристик
        [Header("Charecter Text")]
        [SerializeField] private Text[] _textResistance;
        [SerializeField] private string[] _stringResistance = new string[16] { "Колющий", "Режущий", "дробящий", "рубящий", "огонь"
        ,"яд","лёд","свет","тьма","электрошок","земля","воздух","кровотечение","проклятие","болезни","длительность контроля"};

        [Header("Снарежение")]
        [SerializeField] private ItemSlot Weapon;

        void Start()
        {
            StartCoroutine(RegenHealth(_timeHealthRegen));
            StartCoroutine(RegenEnergy(_timeEnergyRegen));
        }

        void Update()
        {
            _hpText.text = $"{_hpSlider.value} / {_hpSlider.maxValue}";
            _enText.text = $"{_enSlider.value} / {_enSlider.maxValue}";
            _hpSlider.value = _health;
            _enSlider.value = _energy;
            _textHealth.text = $"Здоровье {_health}";
            _textEnergy.text = $"Энергия {_energy}";
            CheckValueResistance();
        }
        public void CheckValueResistance()
        {
            for (int count = 0; count < _stringResistance.Length; count++)
            {
                _textResistance[count].text = $"{_stringResistance[count]} {Resistance[count]}";
            }
        }
        IEnumerator RegenHealth(float time)
        {
            while (true) {
                yield return new WaitForSeconds(time);
                if (_health < _hpSlider.maxValue)
                {
                    _health += _countRegenHealth;
                }
            }
        }
        IEnumerator RegenEnergy(float time)
        {
            while (true) {
                yield return new WaitForSeconds(time);
                if (_energy < _enSlider.maxValue)
                {
                    _energy += _countRegenEnergy;
                }
            }
        }

        public void UpValueResistance(float value)
        {
            for (int count = 0; count < Resistance.Length; count++)
            {
                Resistance[count] += value;
            }
        }
        public void DownValueResistance(float value)
        {
            for (int count = 0; count < Resistance.Length; count++)
            {
                Resistance[count] -= value;
            }
        }
        public void SetMaxHealth(float health)
        {
            _hpSlider.maxValue += health;
            _hpSlider.value = _hpSlider.maxValue;
        }
        public void SetMaxEnergy(float energy)
        {
            _enSlider.maxValue += energy;
            _enSlider.value = _enSlider.maxValue;
        }
        public float GetMaxHealth()
        {
            return Convert.ToInt32(_hpSlider.maxValue);
        }
        public float GetMaxEnergy()
        {
            return Convert.ToInt32(_enSlider.maxValue);
        }
        public float GetHealth()
        {
            return _health;
        }
        public float GetEnergy()
        {
            return _energy;
        }
        public void SetHealth(float health)
        {
            _health = health;
        }
        public void SetHealthPlus(float health)
        {
            _health += health;
        }
        public void SetEnergy(float energy)
        {
            _energy = energy;
        }
        public void SetEnergyPlus(float energy)
        {
            _energy += energy;
        }

        public float[] GetResistance()
        {
            return Resistance;
        }
        public void SetResistance(float[] resistance)
        {
            Array.Copy(resistance, Resistance, Resistance.Length);
        }
        public float GetDamage()
        {
            return _damageFromHand;
        }
        public void SetDamage(float damage)
        {
            _damageFromHand = damage;
        }
        public ItemSlot GetWeapon()
        {
            return Weapon;
        }
    }
}