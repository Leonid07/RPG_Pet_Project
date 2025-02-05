using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    // ���������� ���������� � ���������
    public class ItemSlotUIEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject PanelItemCharacter;
        public PanelInsideCanvas panelCharacterInsideCanvas;
        public GameObject CloseItemOptions;

        public GameObject PanelItemOptions;
        public PanelInsideCanvas panelOptionsInsideCanvas;

        public ItemContainer itemContainer;

        static ItemSlot cellWhereToTransfer;
        PlayerCharacter playerCharacter;
        ItemSlot portableSlot;
        Image slotUI;
        Vector3 dragOffset;
        Vector3 origin;
        Color regularColor;
        Color dragColor;
        bool Switch;
        int originalSiblingIndex;
        Coroutine coroutine;
        Canvas canvas;

        void Awake()
        {

            portableSlot = GetComponent<ItemSlot>();
            canvas = GetComponent<Canvas>();
            slotUI = GetComponent<Image>();
            playerCharacter = GameObject.Find("Player").GetComponent<PlayerCharacter>();
            originalSiblingIndex = transform.GetSiblingIndex();

            origin = transform.localPosition;
            regularColor = slotUI.color;
            dragColor = new Color(regularColor.r, regularColor.g, regularColor.b, 0.3f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cellWhereToTransfer = portableSlot;
            Switch = !Switch;
            coroutine = StartCoroutine(WaitAndPrint());
            if (portableSlot.itemCount != 0)
            {
                PanelItemCharacter.SetActive(true);
                panelCharacterInsideCanvas.PanelCharacterItem(portableSlot.slotItem);
                PanelItemCharacter.transform.position = Input.mousePosition;
                panelCharacterInsideCanvas.PanelInside_Canvas();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cellWhereToTransfer = null;
            if (portableSlot.itemCount != 0)
                PanelItemCharacter.SetActive(false);
            Switch = !Switch;
            StopCoroutine(coroutine);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (portableSlot.slotItem != null) {
                canvas.sortingOrder = 20;
                slotUI.color = dragColor;
                slotUI.raycastTarget = false;
                cellWhereToTransfer = null;
                dragOffset = Input.mousePosition - transform.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (portableSlot.slotItem != null) {
                PanelItemCharacter.SetActive(false);
                transform.position = Input.mousePosition - dragOffset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (cellWhereToTransfer != null)
                if (cellWhereToTransfer.GetItemType() == portableSlot.slotItem.Type || cellWhereToTransfer .GetItemType() == ItemType.Defoult)
                    DressedItem(cellWhereToTransfer, portableSlot);
            canvas.sortingOrder = 2;
            transform.SetSiblingIndex(originalSiblingIndex);
            transform.localPosition = origin;
            slotUI.color = regularColor;
            slotUI.raycastTarget = true;
        }
        enum EquipmentAction
        {
            None,
            PutOn,
            PutOnCellWhereToTransfer
        }
        public void DressedItem(ItemSlot cellWhereToTransfer, ItemSlot portableSlot)
        {
            //�������� �������� �� ������ ���� ���� ��������� ������� Defoult (Defoult ��� ��� ������� �������� ���� ������� � ���������)
            if (cellWhereToTransfer.GetItemType() != ItemType.Defoult)
            {
                // �������� �������� ����� ���������������� �������� � ������� ���� ���� ��������� �������
                if (cellWhereToTransfer.GetItemType() == portableSlot.slotItem.Type)
                {
                    // �������� ���� ������� ������� � ��������������� ������
                    if (cellWhereToTransfer.slotItem != portableSlot.slotItem)
                    {
                        AdditionOfCharacteristics(portableSlot);// ����������� ������������� ���������������� ��������
                        if (cellWhereToTransfer.slotItem != null)// �������� �������� �� ������ ������
                            FeatureSubtraction(cellWhereToTransfer);// ���� ������ �� ������ �� �������� �������������� ����� ����� ��� ����� �������
                    }
                    PutOnCharacter(cellWhereToTransfer, portableSlot, EquipmentAction.PutOn);//���������� ����������� ������� ��������

                    cellWhereToTransfer.isDressed = true;// �������� ������ true ����� ������� ����
                }
                // ����� ����������� ����������� � ���������� ���� �������� � ����� ������ � �������� ������ �� ������ ������� ���������
                Utils.TransferItem(portableSlot, cellWhereToTransfer);
            }
            else
            {
                //������� ����������� ���� ������� ����� ��������� ������� � ���������
                //��������� ���� � �������� ����� ������ true (����)
                if (portableSlot.isDressed == true)
                {
                    cellWhereToTransfer.isDressed = false;// ������ ����� �� false (�����)
                    if (cellWhereToTransfer.slotItem == null)// �������� ���� ����� ���� �� ����� ��������� ������
                    {
                        FeatureSubtraction(portableSlot);// �������� �������������� ���������������� ��������
                        PutOnCharacter(cellWhereToTransfer, portableSlot, EquipmentAction.None);// ��������� ������� � ��������� �������
                        if(portableSlot.slotItem.Type == ItemType.RightHand)
                        itemContainer.carrier.RightHandWeapon = null;
                    }
                    else
                    {
                        //���� �� ����� �������� ������� �� ������ �������
                        FeatureSubtraction(portableSlot);//�������� �������������� ������� �����
                        AdditionOfCharacteristics(cellWhereToTransfer);// ���������� �������������� �������� ������� ���� ������
                        portableSlot.isDressed = true;// � �������� ������� ������� �������� ����� �� true (����)
                        //��������� ������� ������ ������� � ������� �����
                        PutOnCharacter(cellWhereToTransfer, portableSlot, EquipmentAction.PutOnCellWhereToTransfer);
                    }
                }
                // ����� ����������� ����������� � ���������� ���� �������� � ����� ������ � �������� ������ �� ������ ������� ���������
                Utils.TransferItem(portableSlot, cellWhereToTransfer);
            }
        }
        void PutOnCharacter(ItemSlot cellWhereToTransfer, ItemSlot portableSlot, EquipmentAction equipmentAction)
        {
            switch (portableSlot.slotItem.Type)
            {
                case ItemType.LeftHand:
                    if (itemContainer.carrier.LeftHand.transform.childCount > 0)
                        foreach (Transform child in itemContainer.carrier.LeftHand.transform)
                            Destroy(child.gameObject);
                    if (equipmentAction == EquipmentAction.PutOn)
                        Instantiate(portableSlot.slotItem.Prefab, itemContainer.carrier.LeftHand.transform);
                    if (equipmentAction == EquipmentAction.PutOnCellWhereToTransfer)
                        Instantiate(cellWhereToTransfer.slotItem.Prefab, itemContainer.carrier.LeftHand.transform);
                    break;
                case ItemType.RightHand:
                    if (itemContainer.carrier.RightHand.transform.childCount > 0)
                        foreach (Transform child in itemContainer.carrier.RightHand.transform)
                            Destroy(child.gameObject);
                    if (equipmentAction == EquipmentAction.PutOn)
                    {
                        Instantiate(portableSlot.slotItem.Prefab, itemContainer.carrier.RightHand.transform);
                        itemContainer.carrier.RightHandWeapon = portableSlot.slotItem;
                    }
                    if (equipmentAction == EquipmentAction.PutOnCellWhereToTransfer)
                    {
                        Instantiate(cellWhereToTransfer.slotItem.Prefab, itemContainer.carrier.RightHand.transform);
                        itemContainer.carrier.RightHandWeapon = cellWhereToTransfer.slotItem;
                    }
                    break;
                case ItemType.Helmet:
                    if (itemContainer.carrier.Helmet.transform.childCount > 0)
                        foreach (Transform child in itemContainer.carrier.Helmet.transform)
                            Destroy(child.gameObject);
                    if (equipmentAction == EquipmentAction.PutOn)
                        Instantiate(portableSlot.slotItem.Prefab, itemContainer.carrier.Helmet.transform);
                    if (equipmentAction == EquipmentAction.PutOnCellWhereToTransfer)
                        Instantiate(cellWhereToTransfer.slotItem.Prefab, itemContainer.carrier.Helmet.transform);
                    break;
            }
        }
        IEnumerator WaitAndPrint()
        {
            while (true)
            {
                if (Switch)
                {
                    if (Input.GetMouseButtonDown(1) && portableSlot.itemCount != 0)
                    {
                        if (!portableSlot.isDressed) {
                            itemContainer.SetItemSlot(portableSlot);
                            PanelItemOptions.SetActive(true);
                            PanelItemOptions.transform.position = Input.mousePosition;
                            panelOptionsInsideCanvas.PanelInside_Canvas();
                            CloseItemOptions.SetActive(true);
                            PanelItemCharacter.SetActive(false);
                        }
                    }
                }
                yield return null;
            }
        }
        public void AdditionOfCharacteristics(ItemSlot itemSlot)
        {
            for (int quantityResistance = 0; quantityResistance < playerCharacter.GetResistance().Length; quantityResistance++)
            {
                playerCharacter.GetResistance()[quantityResistance] += itemSlot.slotItem._Resistance[quantityResistance];
            }
        }
        public void FeatureSubtraction(ItemSlot itemSlot)
        {
            for (int quantityResistance = 0; quantityResistance < playerCharacter.GetResistance().Length; quantityResistance++)
            {
                playerCharacter.GetResistance()[quantityResistance] -= itemSlot.slotItem._Resistance[quantityResistance];
            }
        }
    }
}