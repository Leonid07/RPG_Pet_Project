using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        [SerializeField] public ItemType Type;//Тип предмета
        [SerializeField] public string ItemName;//имя
        [SerializeField] public string Discription;//описание
        [SerializeField] public int ItemPerSlot;//сколько предметов можно поместить в один слот
        [SerializeField] public Sprite Icon;//иконка предмета в инвентаре
        [SerializeField] public GameObject Prefab;//моделька предмета

        [SerializeField] public bool IsCraft;
        [SerializeField] public Craft[] craft;

        [Header("Параметры для оружия")]
        [SerializeField] TypeDamage typeDamage;//Урон
        [SerializeField] public float[] _TypeDamage;
        [SerializeField] public float CriticalHitChance;//Шанс критического удара
        [SerializeField] public float CriticalDamage;//Критический урон
        [SerializeField] public float AttackSpeed;//Скорость атаки
        [SerializeField] public bool IsTwoHanded;//двуручное или одноручное
        [SerializeField] public AudioClip[] WeaponSound;// звуки которое издаёт оружие при ударах
        [SerializeField] public AudioClip[] TheSoundOfHittingWood;// звуки которое издаёт оружие при ударах
        [SerializeField] public AudioClip[] TheSoundOfHittingStone;// звуки которое издаёт оружие при ударах

        [Header("Параметры для брони")]
        [SerializeField] public bool IsArmorSet;
        [SerializeField] public string NameArmorSet;
        [SerializeField] public ArmorType ArmorType;

        [Header("Параметры для Расходного материала")]
        [SerializeField] public Consumables consumables;
        [SerializeField] public float HealthOrPerSecond;//жизнь
        [SerializeField] public float EnergyOrPerSecond;//энергич
        [SerializeField] public float Timer;

        [Header("Сопротивления")]
        //Часть отвечающая за сопротивления
        [SerializeField] Resistance resistance;//вывести в инспектор
        [SerializeField] public float[] _Resistance;
        //[SerializeField]
        //public string[] _stringResistance = new string[16] { "Pricking", "Cutting", "Crushing", "Chopping", "Fire"
        //,"Poison","Ice","Light","Darkness","Electricity","Ground","Air","Bleeding","Curce","Disease","ControllEffects"};
        [SerializeField]
        public string[] _stringResistancee = new string[16] { "Колющий", "Режущий", "дробящий", "рубящий", "огонь"
        ,"яд","лёд","свет","тьма","электрошок","земля","воздух","кровотечение","проклятие","болезни","длительность контроля"};
        [Serializable]
        public struct Craft
        {
            [SerializeField] public Item WhichSubjects;
            [SerializeField] public int Amount;
        }

        private void OnValidate()
        {
            _Resistance = new float[]
            {
        resistance._pricking,
        resistance._cutting,
        resistance._crushing,
        resistance._chopping,
        resistance._fireResistance,
        resistance._poisonResistance,
        resistance._iceResistance,
        resistance._lightResistance,
        resistance._darknessResistance,
        resistance._electricityResistance,
        resistance._groundResistance,
        resistance._airResistance,
        resistance._bleedingResistance,
        resistance._curceResistance,
        resistance._diseaseResistance,
        resistance._controlEffectsResistance
            };
            _TypeDamage = new float[] {
                typeDamage._pricking,
        typeDamage._cutting,
        typeDamage._crushing,
        typeDamage._chopping,
        typeDamage._fireDamage,
        typeDamage._poisonDamage,
        typeDamage._iceDamage,
        typeDamage._lightDamage,
        typeDamage._darknessDamage,
        typeDamage._electricityDamage,
        typeDamage._groundDamage,
        typeDamage._airDamage,
        typeDamage._bleedingDamage,
        typeDamage._curceDamage,
        typeDamage._diseaseDamage,
        typeDamage._controlEffectsDamage
            };
        }
    }
    [Serializable]
    public class TypeDamage
    {
        public float _pricking;
        public float _cutting;
        public float _crushing;
        public float _chopping;
        public float _fireDamage;
        public float _poisonDamage;
        public float _iceDamage;
        public float _lightDamage;
        public float _darknessDamage;
        public float _electricityDamage;
        public float _groundDamage;
        public float _airDamage;
        public float _bleedingDamage;
        public float _curceDamage;
        public float _diseaseDamage;
        public float _controlEffectsDamage;
    }
    [Serializable]
    public class Resistance
    {
        public float _pricking;
        public float _cutting;
        public float _crushing;
        public float _chopping;
        public float _fireResistance;
        public float _poisonResistance;
        public float _iceResistance;
        public float _lightResistance;
        public float _darknessResistance;
        public float _electricityResistance;
        public float _groundResistance;
        public float _airResistance;
        public float _bleedingResistance;
        public float _curceResistance;
        public float _diseaseResistance;
        public float _controlEffectsResistance;

    }
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

#if UNITY_EDITOR
    [CustomEditor(typeof(Item))]
    public class EnableOrDisableParametrs : Editor
    {
        private Item script;

        //Параметры для оружия
        private SerializedProperty Damage;
        private SerializedProperty CriticalHitChance;
        private SerializedProperty CriticalDamage;
        private SerializedProperty AttackSpeed;
        private SerializedProperty IsTwoHanded;
        private SerializedProperty WeaponSound;
        private SerializedProperty TheSoundOfHittingWood;
        private SerializedProperty TheSoundOfHittingStone;

        private SerializedProperty Сonsumables;
        private SerializedProperty HealthOrPerSecond;
        private SerializedProperty EnergyOrPerSecond;
        private SerializedProperty Timer;

        private SerializedProperty IsArmorSet;
        private SerializedProperty NameArmorSet;
        private SerializedProperty ArmorType;

        private SerializedProperty IsCraft;
        private SerializedProperty Craft;

        private void OnEnable()
        {
            script = (Item)target;

            Damage = serializedObject.FindProperty("typeDamage");
            CriticalHitChance = serializedObject.FindProperty("CriticalHitChance");
            CriticalDamage = serializedObject.FindProperty("CriticalDamage");
            AttackSpeed = serializedObject.FindProperty("AttackSpeed");
            IsTwoHanded = serializedObject.FindProperty("IsTwoHanded");
            WeaponSound = serializedObject.FindProperty("WeaponSound");
            TheSoundOfHittingWood = serializedObject.FindProperty("TheSoundOfHittingWood");
            TheSoundOfHittingStone = serializedObject.FindProperty("TheSoundOfHittingStone");

            Сonsumables = serializedObject.FindProperty("consumables");
            HealthOrPerSecond = serializedObject.FindProperty("HealthOrPerSecond");
            EnergyOrPerSecond = serializedObject.FindProperty("EnergyOrPerSecond");
            Timer = serializedObject.FindProperty("Timer");

            IsArmorSet = serializedObject.FindProperty("IsArmorSet");
            NameArmorSet = serializedObject.FindProperty("NameArmorSet");
            ArmorType = serializedObject.FindProperty("ArmorType");

            IsCraft = serializedObject.FindProperty("IsCraft");
            Craft = serializedObject.FindProperty("craft");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (script == null)
            {
                EditorGUILayout.LabelField("Item is null");
                return;
            }
            string[] variableNames =
            {
                "Prefab",
                "ItemName",
                "Discription",
                "ItemPerSlot",
                "Icon",
                "resistance",
                "Type"
            };
            foreach (var variableName in variableNames)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(variableName));

            EditorGUILayout.PropertyField(IsCraft);
            if (IsCraft.boolValue)
            {
                EditorGUILayout.PropertyField(Craft);
            }

            ItemType selectedType = script.Type;
            switch (selectedType)
            {
                case ItemType.Bib:
                case ItemType.Bijouterie:
                case ItemType.LeftHand:
                case ItemType.Helmet:
                case ItemType.Shoulders:
                case ItemType.Mittens:
                case ItemType.Leggings:
                case ItemType.Boots:
                    EditorGUILayout.PropertyField(IsArmorSet);
                    EditorGUILayout.PropertyField(NameArmorSet);
                    EditorGUILayout.PropertyField(ArmorType);
                    break;
                case ItemType.RightHand:
                    EditorGUILayout.PropertyField(Damage);
                    EditorGUILayout.PropertyField(CriticalHitChance);
                    EditorGUILayout.PropertyField(CriticalDamage);
                    EditorGUILayout.PropertyField(AttackSpeed);
                    EditorGUILayout.PropertyField(IsTwoHanded);
                    EditorGUILayout.PropertyField(WeaponSound);
                    EditorGUILayout.PropertyField(TheSoundOfHittingWood);
                    EditorGUILayout.PropertyField(TheSoundOfHittingStone);
                    break;
                case ItemType.Consumable:
                    EditorGUILayout.PropertyField(Сonsumables);
                    EditorGUILayout.PropertyField(HealthOrPerSecond);
                    EditorGUILayout.PropertyField(EnergyOrPerSecond);
                    EditorGUILayout.PropertyField(Timer);
                    break;
                case ItemType.Material:

                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}