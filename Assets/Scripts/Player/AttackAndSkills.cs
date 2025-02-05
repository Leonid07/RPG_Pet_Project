using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using System;
using DebuffBuff;
namespace Player
{
    public class AttackAndSkills : MonoBehaviour
    {
        public GameObject Player;
        public ItemContainer itemContainer;
        public WeaponDamageUp WeaponDamageUp;
        public AudioClip audioClip;

        private int _animIDAttackSmash;
        private int _animIDBlock;
        private int _animIDKick;
        private int _animIDPowerUp;

        private PlayerMovetment playerMovetment;
        private PlayerCharacter playerCharacter;
        private StarterAssetsInputs _input;
        private Animator _animator;
        private Interactor interactor;

        private CapsuleCollider damageZoneAutoAttack;

        [Header("Параметры для способности PowerUp")]
        [SerializeField] float _timeReloadPowerUp;
        [SerializeField] float _energyCostPowerUp;
        [Header("Параметры для лечения")]
        [SerializeField] float _countHealth;
        [SerializeField] int _timerHeal;
        [SerializeField] GameObject prefabHeal;
        [Header("Параметры для увеличения сопротивления")]
        [SerializeField] int[] _indexResistance;
        [SerializeField] float[] _countResistance;
        [SerializeField] int _timerResist;
        [SerializeField] GameObject prefabRaiseResistance;
        [Header("Параметры для увеличения урона")]
        [SerializeField] float _percentReiseDamage;
        [SerializeField] int _timerReiseDamage;
        [SerializeField] GameObject prefabRaiseDamage;
        public bool isReady = true;

        private void Start()
        {
            playerMovetment = Player.GetComponent<PlayerMovetment>();
            playerCharacter = Player.GetComponent<PlayerCharacter>();
            _input = Player.GetComponent<StarterAssetsInputs>();
            _animator = Player.GetComponent<Animator>();
            interactor = Player.GetComponent<Interactor>();

            damageZoneAutoAttack = GetComponent<CapsuleCollider>();
            AssignAnimationIDs();
        }
        private void Update()
        {
            AttackSlash();
            Block();
            Kick();
            PowerUp();
        }
        private void AssignAnimationIDs()
        {
            _animIDAttackSmash = Animator.StringToHash("AttackSlash");
            _animIDBlock = Animator.StringToHash("Block");
            _animIDKick = Animator.StringToHash("Kick");
            _animIDPowerUp = Animator.StringToHash("PowerUp");
        }
        private void AttackSlash()
        {
            if (playerMovetment.Grounded)
            {
                if (playerMovetment._hasAnimator)
                    _animator.SetBool(_animIDAttackSmash, false);

                if (_input.attack)
                {
                    if (playerMovetment._hasAnimator)
                    {
                        _animator.SetBool(_animIDAttackSmash, true);
                        _input.attack = false;
                    }
                }
            }
        }
        private void Block()
        {
            if (playerMovetment.Grounded)
            {
                if (playerMovetment._hasAnimator)
                    _animator.SetBool(_animIDBlock, false);

                if (_input.block == true)
                {
                    _animator.SetBool(_animIDBlock, true);
                }
                else
                {
                    _input.block = false;
                }
            }
        }
        private void Kick()
        {
            if (playerMovetment.Grounded)
            {
                if (playerMovetment._hasAnimator)
                    _animator.SetBool(_animIDKick, false);

                if (_input.kick)
                {
                    if (playerMovetment._hasAnimator)
                    {
                        _animator.SetBool(_animIDKick, true);
                        _input.kick = false;
                    }
                }
            }
        }
        private void PowerUp()
        {
            if (playerMovetment.Grounded)
            {
                if (playerMovetment._hasAnimator)
                    _animator.SetBool(_animIDPowerUp, false);

                if (_input.powerUp && isReady == true)
                {
                    if (playerMovetment._hasAnimator)
                    {
                        if (playerCharacter.GetEnergy() >= _energyCostPowerUp)
                        {
                            playerCharacter.SetEnergyPlus(-_energyCostPowerUp);
                            _animator.SetBool(_animIDPowerUp, true);
                            _input.powerUp = false;
                            isReady = false;
                            StartCoroutine(Buff.HealPerSecond(_countHealth, _timerHeal, playerCharacter, prefabHeal));
                            StartCoroutine(Buff.RaiseResistanceForTime(_indexResistance, _countResistance,
                                _timerResist, playerCharacter, prefabRaiseResistance));
                            StartCoroutine(Buff.RaiseDamageForTime(_percentReiseDamage, _timerReiseDamage,
                                playerCharacter, prefabRaiseDamage));
                            StartCoroutine(CoolDownPowerUp(_timeReloadPowerUp));
                        }
                    }
                }
            }
        }
        IEnumerator CoolDownPowerUp(float time)
        {
            yield return new WaitForSeconds(time);
            _input.powerUp = false;
            isReady = true;
        }
        public void ActiveOrDisactiveCollader()
        {
            damageZoneAutoAttack.enabled = true;
        }

        public AudioClip Weapon_Sound(string typeEnemy = null)
        {
            int index = 0;
            if (typeEnemy == null)
            {
                return audioClip = interactor.Swing;
            }
            if (interactor.RightHandWeapon != null)
            {
                switch (typeEnemy)
                {
                    case "Tree":
                        index = UnityEngine.Random.Range(0, interactor.RightHandWeapon.TheSoundOfHittingWood.Length);
                        audioClip = interactor.RightHandWeapon.TheSoundOfHittingWood[index];
                        return audioClip;
                    case "Enemy":
                        index = UnityEngine.Random.Range(0, interactor.RightHandWeapon.WeaponSound.Length);
                        audioClip = interactor.RightHandWeapon.WeaponSound[index];
                        return audioClip;
                    case "Rock":
                        index = UnityEngine.Random.Range(0, interactor.RightHandWeapon.TheSoundOfHittingStone.Length);
                        audioClip = interactor.RightHandWeapon.TheSoundOfHittingStone[index];
                        return audioClip;
                }
            }
            if(interactor.RightHandWeapon == null && typeEnemy.Length > 1)
            {
                audioClip = interactor.PunchSound;
                return audioClip;
            }
            return audioClip = interactor.Swing;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Tree":
                    other.GetComponent<MiningAndCreation>().SetMinusHP(1, itemContainer);
                    other.enabled = false;
                    Weapon_Sound("Tree");
                    break;
                case "Enemy":
                    if (itemContainer.carrier.RightHandWeapon != null)
                    {
                        int[] indexArray = new int[itemContainer.carrier.RightHandWeapon._TypeDamage.Length];
                        float[] typeDamageValue = new float[itemContainer.carrier.RightHandWeapon._TypeDamage.Length];
                        int index = 0;
                        for (int i = 0; i < itemContainer.carrier.RightHandWeapon._TypeDamage.Length; i++)
                        {
                            if (itemContainer.carrier.RightHandWeapon._TypeDamage[i] > 0)
                            {
                                indexArray[index] = i;
                                if (WeaponDamageUp.DamageMultiplier >= 1)
                                {
                                    float difference = 1 + (WeaponDamageUp.DamageMultiplier / 100);
                                    typeDamageValue[index] = itemContainer.carrier.RightHandWeapon._TypeDamage[i] * difference;
                                }
                                else
                                {
                                    typeDamageValue[index] = itemContainer.carrier.RightHandWeapon._TypeDamage[i];
                                }
                                index++;
                            }
                        }
                        Array.Resize(ref indexArray, index);
                        Array.Resize(ref typeDamageValue, index);

                        Debuff.EnemyDamageOneShot(other.GetComponent<EnemyCharacter>().GetResistance(), indexArray, typeDamageValue, null, other.GetComponent<EnemyCharacter>());
                    }
                    else
                    {
                        other.GetComponent<EnemyCharacter>().SetHealthMinus(playerCharacter.GetDamage());
                    }
                    Weapon_Sound("Enemy");
                    break;
                case "Rock":
                    other.GetComponent<MiningAndCreation>().SetMinusHP(1, itemContainer);
                    Weapon_Sound("Rock");
                    other.enabled = false;
                    break;
                default:
                    Weapon_Sound();
                    break;
            }
            damageZoneAutoAttack.enabled = false;
        }
    }
}