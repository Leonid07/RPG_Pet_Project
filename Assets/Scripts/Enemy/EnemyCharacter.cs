using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using DebuffBuff;
using System;
using Inventory;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyCharacter : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent; // Ссылка на компонент NavMeshAgent
        Transform _transformPlayer; // Ссылка на трансформ игрока
        Animator _animator; // Ссылка на компонент анимации

        [SerializeField] EnemyAttackAndSkill enemyAttackAndSkill;
        [Header("Основные характеристики персонажа")]
        [SerializeField] float _damage = 10; // Значение урона
        [SerializeField] float _health = 100; // Значение здоровья
        float MaxHealth;

        [Tooltip("// 0 //_pricking;//колющий\n// 1 //_cutting;//режущий\n// 2 //_crushing;//дробящий\n// 3 //_chopping;//рубящий\n// 4 //_fireResistance;//огонь\n// 5 //_poisonResistance;//яд\n// 6 //_iceResistance;//лёд\n// 7 //_lightResistance;//свет\n// 8 //_darknessResistance;//тьма\n// 9 //_electricityResistance;//электрошок\n// 10 //_groundResistance;//земля\n// 11 //_airResistance;//воздух\n// 12 //_bleedingResistance;//кровотечение\n// 13 //_curceResistance;// проклятие\n// 14 //_diseaseResistance;// болезни\n// 15 //_controlEffectsResistance;// длительность контроля")]
        [SerializeField] private float[] Resistance = new float[16];
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
        [SerializeField] float _attackDistance = 2f; // Расстояние для атаки
        [SerializeField] CapsuleCollider DamageZone;
        [SerializeField] CapsuleCollider _body;

        [SerializeField] float _speedWalk = 2.5f; // Скорость ходьбы
        [SerializeField] float _speedRun = 4f; // Скорость бега
        [SerializeField] float _chaseRadius = 10f; // Радиус преследования игрока

        float currentSpeed; // Текущая скорость объекта

        [Header("Предметы которые выпадают послу смерти монстра")]
        public DropItems[] dropItems;

        // Патрулирование
        [Space(10)]
        [Header("Параметры патрулирования и времени ожидания на точках")]
        [SerializeField] float minIdleTime = 2f; // Минимальное время ожидания на точке (в секундах)
        [SerializeField] float maxIdleTime = 5f; // Максимальное время ожидания на точке (в секундах)
        [SerializeField] float patrolRadius = 10f; // Радиус поиска случайных точек

        [Space(10)]
        [Header("Навык 'Жажда крови'")]
        [Header("Параметры для повышения урона")]
        [SerializeField] float RaiseDamagePercent;
        [SerializeField] int TimerRaiseDamage;
        [Header("Параметры для повышения сопротивления")]
        [SerializeField] int[] _indexResistance;
        [SerializeField] float[] _countResistance;
        [SerializeField] float _timerResistance;
        float timerResistance;

        EnemyCharacter enemyCharacter;
        CapsuleCollider capsuleCollider;

        [Space(10)]
        [Header("Параметры настройки звуков")]
        //public Slider sliderVolume;
        public AudioClip[] FootstepAudioClips;
        public AudioClip ScreamSound;
        AudioSource audioSource;

        bool Move = true; // Флаг движения
        bool isDeath = false; // Флаг смерти
        bool isAttack = false; // Флаг атаки
        bool isAttackRange = false; // Флаг в радиусе атаки
        bool isDropItems = false; // Флаг были ли выброшены предметы
        public bool isScream = false;

        Vector3 targetPosition; // Позиция целевой точки для патрулирования
        float idleTimer = 0f; // Таймер ожидания на точке
        float currentIdleTime = 0f; // Текущее время ожидания на точке

        int _animIDSpeed;
        int _animIDIsAttack;
        int _animIDAttackWarriant;
        int _animIDIsScream;

        float distanceToPlayer;

        [System.Serializable]
        public struct DropItems
        {
            public Item itemToDrop;
            [Range(0, 1)]
            public float chance;
            public int count; //Количество предметов которые могут выпасть
        }
        void Start()
        {
            _transformPlayer = GameObject.Find("Player").transform; // Поиск и назначение трансформа игрока
            _navMeshAgent = GetComponent<NavMeshAgent>(); // Получение компонента NavMeshAgent
            _animator = GetComponent<Animator>();// Получение компонента Animator
            currentSpeed = _speedWalk; // Инициализация текущей скорости
            MaxHealth = _health;
            timerResistance = _timerResistance;
            _timerResistance = 0;
            capsuleCollider = GetComponent<CapsuleCollider>();
            enemyCharacter = GetComponent<EnemyCharacter>();
            audioSource = GetComponent<AudioSource>();
            AssignAnimationIDs();
            Death(transform);
        }

        void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDIsAttack = Animator.StringToHash("IsAttacking");
            _animIDAttackWarriant = Animator.StringToHash("WariantsAttack");
            _animIDIsScream = Animator.StringToHash("IsScream");
        }
        void Update()
        {
            EnemyController();
            Dead(transform);
                DropItem();
        }

        void EnemyController()
        {
            distanceToPlayer = Vector3.Distance(transform.position, _transformPlayer.position); // Расстояние до игрока

            _timerResistance = Mathf.Max(_timerResistance - Time.deltaTime, 0);

            if (distanceToPlayer <= _chaseRadius) // Если игрок в радиусе преследования и разрешено двигаться
            {
                _navMeshAgent.isStopped = false; // Включение NavMeshAgent

                if (_timerResistance == 0)
                {
                    _animator.SetBool(_animIDIsScream, true);
                    currentSpeed = 0;
                    _navMeshAgent.speed = 0;
                    _animator.SetFloat(_animIDSpeed, 0);
                    _timerResistance = timerResistance;
                }

                if (distanceToPlayer > _attackDistance)
                {
                    if (isScream == false)
                    {
                        // Увеличение скорости при приближении к игроку
                        currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * 3f, _speedRun);
                        _navMeshAgent.speed = currentSpeed;
                        isAttackRange = false;
                        _animator.SetFloat(_animIDSpeed, currentSpeed);
                        _animator.SetBool(_animIDIsAttack, false); // Прекращение анимации атаки
                        _navMeshAgent.destination = _transformPlayer.position; // Установка цели на игрока
                    }
                }
                else
                {
                    _animator.SetBool(_animIDIsAttack, true); // Запуск анимации атаки
                    isAttackRange = true;
                    currentSpeed = 0;
                    _navMeshAgent.speed = 0;
                    _animator.SetFloat(_animIDSpeed, 0);
                    _navMeshAgent.destination = transform.position; // Остановка перед атакой
                }
            }
            else
            {
                if (!Move)
                {
                    idleTimer += Time.deltaTime;
                    currentSpeed = Mathf.Max(currentSpeed - Time.deltaTime * 5f, 0); // Уменьшение скорости при ожидании
                    _navMeshAgent.speed = currentSpeed;
                    _animator.SetFloat(_animIDSpeed, currentSpeed);

                    if (idleTimer >= currentIdleTime)
                    {
                        SetRandomDestination(); // Выбор новой цели для патрулирования
                    }
                }
                else
                {
                    if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    {
                        _navMeshAgent.isStopped = true; // Остановка при достижении цели
                        Move = false;
                        currentIdleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime); // Установка случайного времени ожидания
                        idleTimer = 0f;
                    }
                    else
                    {
                        currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * 3f, _speedWalk); // Увеличение скорости при движении
                        _navMeshAgent.speed = currentSpeed;
                        _animator.SetFloat(_animIDSpeed, currentSpeed);
                    }
                }
            }
        }
        void DropItem()
        {
            if (isDeath == true && isDropItems == false)
            {
                for (int i = 0; i < dropItems.Length; i++)
                {
                    System.Random prng = new System.Random(GetHashCode());
                    DropItems drop = dropItems[i];
                    if (prng.NextDouble() <= drop.chance)
                    {
                        for (int count = 0; count < drop.count; count++)
                        {
                            if (!_transformPlayer.GetComponent<Interactor>().inventory.SearchForAnEmptyCell())
                            {
                                Utils.InstantiateItemCollector(drop.itemToDrop, gameObject.transform.position);
                            }
                            else
                            {
                                _transformPlayer.GetComponent<Interactor>().inventory.AddItem(drop.itemToDrop);
                            }
                        }
                    }
                }
                isDropItems = true;
            }
        }
        void SkillBloodLust()
        {
            StartCoroutine(Buff.RaiseDamageForTime(RaiseDamagePercent, TimerRaiseDamage, null,null, GetComponent<EnemyCharacter>()));
            StartCoroutine(Buff.RaiseResistanceForTime(_indexResistance, _countResistance, _timerResistance,null,null, GetComponent<EnemyCharacter>()));
            _animator.SetBool(_animIDIsScream, false);
        }
        void Screaming()
        {
            isScream = !isScream;
        }
        void SetRandomDestination()
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius; // Генерация случайного направления
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
            targetPosition = hit.position; // Новая целевая точка для патрулирования
            _navMeshAgent.SetDestination(targetPosition);
            _navMeshAgent.isStopped = false;
            Move = true; // Разрешение двигаться
        }
        void Dead(Transform parent)
        {
            if (_health <= 0)
            {
                _body.isTrigger = true;
                isDeath = true;
                foreach (Transform child in parent)
                {
                    // Обрабатываем дочерний объект
                    Rigidbody rb = child.GetComponent<Rigidbody>();
                    //Collider col = child.GetComponent<Collider>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.useGravity = true;
                    }

                    // Рекурсивно вызываем функцию для дочерних объектов
                    Dead(child);
                }
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Animator>().enabled = false;
                GetComponent<EnemyCharacter>().enabled = false;
            }
        }
        void Death(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // Обрабатываем дочерний объект
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // Рекурсивно вызываем функцию для дочерних объектов
                Death(child);
            }
        }
        void OnFootstepSound(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    audioSource.clip = FootstepAudioClips[index];
                    audioSource.Play();
                }
            }
        }
        void OnScreamSound(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                audioSource.clip = ScreamSound;
                audioSource.Play();
            }
        }
        void OnSoundAttack(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                enemyAttackAndSkill.OnScreamSound();
            }
        }
        void WariantAttack()
        {
            _animator.SetFloat(_animIDAttackWarriant, UnityEngine.Random.Range(0, 2)); // Выбор случайного варианта атаки
        }
        void ColliderManipulationActive()
        {
            DamageZone.enabled = true;
        }
        void ColliderManipulationDisactive()
        {
            DamageZone.enabled = false;
        }
        public void SetHealthMinus(float health)
        {
            _health -= health;
        }
        public void SetHealthPlus(float health)
        {
            _health += health;
        }
        public float GetHealth()
        {
            return _health;
        }
        public void SetHealth(float health)
        {
            _health = health;
        }
        public float GetMaxHealth()
        {
            return MaxHealth;
        }
        public bool GetInAttackRange()
        {
            return isAttackRange;
        }
        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        //public float GetDamage()
        //{
        //    return _damage;
        //}
        //public void SetDamage(float damage)
        //{
        //    _damage = damage;
        //}
        public bool GetIsDead()
        {
            return isDeath;
        }
        public float[] GetResistance()
        {
            return Resistance;
        }
        public void SetResistance(float[] resistance)
        {
            Array.Copy(resistance, Resistance, Resistance.Length);
        }
    }
}