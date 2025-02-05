using Inventory;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("���������� �������������")]
        [SerializeField] private float _health;//��������
        [SerializeField] private float _energy;//�������
        [SerializeField] float _damageFromHand = 5f;

        [SerializeField] float _countRegenHealth;
        [SerializeField] float _timeHealthRegen;
        [SerializeField] float _countRegenEnergy;
        [SerializeField] float _timeEnergyRegen;

        [Header("�������������")]
        [SerializeField] private float[] Resistance = new float[16];
        [SerializeField] private Text _textHealth;
        [SerializeField] private Text _textEnergy;
        #region
        // 0 //_pricking;//�������
        // 1 //_cutting;//�������
        // 2 //_crushing;//��������
        // 3 //_chopping;//�������
        // 4 //_fireResistance;//�����
        // 5 //_poisonResistance;//��
        // 6 //_iceResistance;//��
        // 7 //_lightResistance;//����
        // 8 //_darknessResistance;//����
        // 9 //_electricityResistance;//����������
        // 10 //_groundResistance;//�����
        // 11 //_airResistance;//������
        // 12 //_bleedingResistance;//������������
        // 13 //_curceResistance;// ���������
        // 14 //_diseaseResistance;// �������
        // 15 //_controlEffectsResistance;// ������������ ��������
        #endregion
        [Space(20)]
        //�������� ������� ������ ����
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _enSlider;

        // �������������� � ������ ������
        [SerializeField] private Text _hpText;
        [SerializeField] private Text _enText;
        public GameObject panelAtribute;

        //������ �������������
        [Header("Charecter Text")]
        [SerializeField] private Text[] _textResistance;
        [SerializeField] private string[] _stringResistance = new string[16] { "�������", "�������", "��������", "�������", "�����"
        ,"��","��","����","����","����������","�����","������","������������","���������","�������","������������ ��������"};

        [Header("����������")]
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
            _textHealth.text = $"�������� {_health}";
            _textEnergy.text = $"������� {_energy}";
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