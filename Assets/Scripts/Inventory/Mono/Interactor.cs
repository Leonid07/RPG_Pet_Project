using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    //���� ����� ���������� � ������.
    //����� �������� ��������� ���� ������� �������������� � ������ �������� ��������������.
    public class Interactor : MonoBehaviour
    {
        public AttackAndSkills attackAndSkills;
        public GameObject LeftHand;
        public GameObject RightHand;
        public GameObject Helmet;
        public Item LeftHandWeapon;
        public Item RightHandWeapon;
        public Item HelmetWeapon;

        [Tooltip("����������� ���� ����� �� ���� ���� �����")]
        public AudioClip PunchSound;
        public AudioClip Swing;
        public AudioClip SkreamPowerUpClip;
        AudioSource audioSource;

        //����������� ���������� �� ������� ��� ��������������.
        [SerializeField] private float interactionRange;

        //������ �� �������� ������ ��������� ����.
        [SerializeField] private Camera mainCamera;

        //������ �� ��������� ���������, ��������������� ��� ����� �����������.
        public ItemContainer inventory;

        //������ �� ������� ������������� ����.
        //��� �������� ����� ����, ���� ��� ���������� ������� ������������� ��������.
        private Interactable interactionTarget;

        //��������� ��������� ���������, ������ ����������� ����� ��� ����������� ��������� �������������� � ���������.
        [Header("Settings")]
        public bool drawRangeIndicator;

        //��� ���� ���������� ������������� ��������.
        //��� ���� ����� ������������� ������� ����� ���� ��������, ��� ������ ����� �������� ������������ ����� � ���������� ����������.
        public Color interactableHighlight = Color.white;

        //��� �������, � ������� ����� ������� ���������� ����������� ��������� (����� ���� ������������).
        public Vector3 ItemDropPosition { get { return transform.position + transform.forward + transform.up; } }

        Ray ray;
        RaycastHit hit;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            ray = new Ray();
            hit = new RaycastHit();
        }
        //���������� ������ ���� ����� ������� ����.
        private void Update()
        {
            HandleInteractions();
        }

        //���� ����� ������ ��������� � ���������.
        private void OnDrawGizmos()
        {
            if (drawRangeIndicator)
            {
                Gizmos.DrawWireSphere(transform.position, interactionRange);
            }
        }

        //���� ����� ������������ ����������� ������������� ��������, ������� �������������� � �������� ������ ������� ��������������.
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

        //��� ���������� true, ���� ������� ������� ��������� � ��������� ��������������.
        private bool InRange(Vector3 targetPosition)
        {
            return Vector3.Distance(targetPosition, transform.position) <= interactionRange;
        }

        //���� ����� �������������� �������������� � ���� ������������, ���� ���������� ���������� ���� ��������������.
        private void InitInteraction()
        {
            if (interactionTarget == null) return;
            interactionTarget.OnInteract(this);
        }

        //���� ����� ��������� �������� � ��������� ����� ����������� �, ���� ���������, ���������� ���������� ��������� ��������.
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