using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    //Этот класс прикреплен к игроку.
    //Здесь хранятся различные типы событий взаимодействия и методы триггера взаимодействия.
    public class Interactor : MonoBehaviour
    {
        public AttackAndSkills attackAndSkills;
        public GameObject LeftHand;
        public GameObject RightHand;
        public GameObject Helmet;
        public Item LeftHandWeapon;
        public Item RightHandWeapon;
        public Item HelmetWeapon;

        [Tooltip("Стандартный звук удара то есть удар рукой")]
        public AudioClip PunchSound;
        public AudioClip Swing;
        public AudioClip SkreamPowerUpClip;
        AudioSource audioSource;

        //Минимальное расстояние до объекта для взаимодействия.
        [SerializeField] private float interactionRange;

        //Ссылка на основную камеру просмотра игры.
        [SerializeField] private Camera mainCamera;

        //Ссылка на контейнер элементов, предназначенный для этого интерактора.
        public ItemContainer inventory;

        //Ссылка на текущую интерактивную цель.
        //Это значение равно нулю, если нет допустимых целевых интерактивных объектов.
        private Interactable interactionTarget;

        //Активируя индикатор диапазона, рисует проволочную сферу для обозначения диапазона взаимодействия в редакторе.
        [Header("Settings")]
        public bool drawRangeIndicator;

        //Это цвет выделенных интерактивных объектов.
        //Для того чтобы интерактивные объекты могли быть выделены, они должны иметь средство визуализации сетки с допустимым материалом.
        public Color interactableHighlight = Color.white;

        //Это позиция, в которой будут созданы экземпляры отброшенных элементов (перед этим интерактивом).
        public Vector3 ItemDropPosition { get { return transform.position + transform.forward + transform.up; } }

        Ray ray;
        RaycastHit hit;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            ray = new Ray();
            hit = new RaycastHit();
        }
        //Вызывается каждый кадр после запуска игры.
        private void Update()
        {
            HandleInteractions();
        }

        //Этот метод рисует штуковины в редакторе.
        private void OnDrawGizmos()
        {
            if (drawRangeIndicator)
            {
                Gizmos.DrawWireSphere(transform.position, interactionRange);
            }
        }

        //Этот метод обрабатывает обнаружение интерактивных объектов, триггер взаимодействия и обратные вызовы событий взаимодействия.
        private void HandleInteractions()
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (interactionTarget)
            {
                Utils.UnhighlightObject(interactionTarget.gameObject);
                interactionTarget.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (Physics.Raycast(ray, out hit) && InRange(hit.transform.position))
            {
                Interactable target = hit.transform.GetComponent<Interactable>();
                if (target != null)
                {
                    interactionTarget = target;
                    Utils.HighlightObject(interactionTarget.gameObject, interactableHighlight);
                    interactionTarget.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    interactionTarget = null;
                }
            }
            else
            {
                interactionTarget = null;
            }

            if (Input.GetMouseButtonDown(0)) InitInteraction();
        }

        //Это возвращает true, если целевая позиция находится в диапазоне взаимодействия.
        private bool InRange(Vector3 targetPosition)
        {
            return Vector3.Distance(targetPosition, transform.position) <= interactionRange;
        }

        //Этот метод инициализирует взаимодействие с этим интерактором, если существует допустимая цель взаимодействия.
        private void InitInteraction()
        {
            if (interactionTarget == null) return;
            interactionTarget.OnInteract(this);
        }

        //Этот метод добавляет элементы в инвентарь этого интерактора и, если применимо, уничтожает физический экземпляр элемента.
        public void AddToInventory(Item item, GameObject instance)
        {
            if (inventory.AddItem(item))
                if (instance)
                {
                    Utils.AnimateObjectDisappear(instance, 1);
                }
        }
        void WeaponSound()
        {
            audioSource.clip = attackAndSkills.audioClip;
            audioSource.Play();
        }
        void OnPowerUpSound()
        {
            audioSource.clip = SkreamPowerUpClip;
            audioSource.Play();
        }
    }
}