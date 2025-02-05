using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebuffBuff;
using System;
using Player;
namespace Enemy
{
    public class EnemyAttackAndSkill : MonoBehaviour
    {
        [SerializeField] EnemyCharacter enemyCharacter;

        [Header("Параметры для удара по сопротивлениям")]
        [SerializeField] int[] ResistanceTypeIndices;
        [SerializeField] float[] ResistanceTypeDamage;

        [Header("Параметры для тикающего урона")]
        [SerializeField] int[] ResistanceTypeIndicesForDOT;
        [SerializeField] float[] ResistanceTypeDamageDOT;
        [SerializeField] float KoolDownDOT = 6f;
        private float timeCoolDown;
        [SerializeField] float TimeDOT = 4;
        [SerializeField] float HealthPercentage = 75f;
        [SerializeField] GameObject prefabAtribute;

        [Header("Параметры для Debuff")]
        [SerializeField] int[] ResistanceTypeIndicesForDebuff;
        [SerializeField] float[] DecreaseTypeResistanceDebuff;
        [SerializeField] float TimeDebuff = 5;
        [SerializeField] GameObject prefabAtributeDebuff;

        [Header("Параметры для звуков")]
        [SerializeField] AudioClip[] _attackSound;

        private PlayerCharacter playerCharacter;
        private CapsuleCollider capsuleColliderDamageZone;
        AudioSource audioSource;

        private void Start()
        {
            timeCoolDown = KoolDownDOT;
            capsuleColliderDamageZone = GetComponent<CapsuleCollider>();
            audioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            if (playerCharacter != null)
            {
                if (enemyCharacter.GetInAttackRange() && !enemyCharacter.GetIsDead())
                    if (HealthPercentage <= (HealthPercentage * playerCharacter.GetMaxHealth()) / 100 && KoolDownDOT == 0)
                    {
                        StartCoroutine(Debuff.DOT(ResistanceTypeDamageDOT, playerCharacter.GetResistance(), ResistanceTypeIndicesForDOT, TimeDOT, prefabAtribute, playerCharacter));
                        StartCoroutine(Debuff.DebuffPerSecond(playerCharacter.GetResistance(), ResistanceTypeIndicesForDebuff, DecreaseTypeResistanceDebuff, TimeDebuff, prefabAtributeDebuff));
                        KoolDownDOT = timeCoolDown;
                    }
                KoolDownDOT = Mathf.Max(KoolDownDOT - Time.deltaTime, 0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerCharacter = other.GetComponent<PlayerCharacter>();
                Debuff.EnemyDamageOneShot(playerCharacter.GetResistance(), ResistanceTypeIndices, ResistanceTypeDamage, playerCharacter);
                capsuleColliderDamageZone.enabled = false;
            }
        }
        public void OnScreamSound()
        {
            var index = UnityEngine.Random.Range(0, _attackSound.Length);
            audioSource.clip = _attackSound[index];
            audioSource.Play();
        }
    }
}