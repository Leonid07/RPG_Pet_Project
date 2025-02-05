using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Player;
using System.Linq;
using System.Collections;

namespace Inventory
{
    // Класс который описывает сам инвентарь
    public class ItemContainer : ItemManager
    {
        public Interactor carrier;
        public bool dropRemovedItemPrefabs = true;
        public GameObject PanelOptions;
        public GameObject PanelOptionsFirstState;
        public GameObject PanelOptionsSecondState;
        public GameObject PanelCloseOptions;
        public Transform PanelInventoryInHero;
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private ItemSlot itemSlot;

        [SerializeField] private ItemSlot[] _slots;
        [SerializeField] private List<ItemSlot> _slotsInHero;

        public Button ButtonUsage;
        public Button ButtonSeparation;
        public Button ButtonThrowAway;
        public Button ButtonDelete;
        public Button ButtonAccept;

        public Button ButtonPlus;
        public Button ButtonMinus;

        public InputField DecreaseOrIncreaseText;
        public enum ItemSettingsStatus
        {
            Separation,
            ThrowAway,
            Delete
        }
        public ItemSettingsStatus itemSettingsStatus;


        protected virtual void Awake()
        {
            InitContainer();
            ButtonUsage.onClick.AddListener(() => Usage(itemSlot));
            ButtonSeparation.onClick.AddListener(() => Separation());
            ButtonThrowAway.onClick.AddListener(() => ThrowAway());
            ButtonDelete.onClick.AddListener(() => Delete());
            ButtonAccept.onClick.AddListener(() => Accept(Convert.ToInt32(DecreaseOrIncreaseText.text), itemSlot));

            ButtonPlus.onClick.AddListener(AddValue);
            ButtonMinus.onClick.AddListener(SubtractValue);
        }

        private void InitContainer()
        {
            _slots = new ItemSlot[transform.childCount];
            for (int i = 0; i < _slots.Length; i++)
            {
                ItemSlot slot = transform.GetChild(i).GetChild(0).GetComponent<ItemSlot>();
                _slots[i] = slot;
            }
            for (int i = 0; i < PanelInventoryInHero.childCount; i++)
            {
                if (PanelInventoryInHero.GetChild(i).childCount >= 1)
                {
                    ItemSlot slot = PanelInventoryInHero.GetChild(i).GetChild(0).GetComponent<ItemSlot>();
                    if (slot != null)
                    {
                        _slotsInHero.Add(slot);
                    }
                }
            }
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < _slots.Length; i++) if (_slots[i].Add(item)) return true;
            return false;
        }

        public bool ContainsItem(Item item)
        {
            for (int i = 0; i < _slots.Length; i++)
                if (_slots[i].slotItem == item) return true;
            return false;
        }

        public bool ContainsItemQuantity(Item item, int amount)
        {
            int count = 0;
            foreach (ItemSlot slot in _slots)
            {
                if (slot.slotItem == item) count += slot.itemCount;
                if (count >= amount) return true;
            }
            return false;
        }
        public bool SearchForAnEmptyCell()
        {
            return _slots.Any(slot => slot.slotItem == null);
        }

        private void Usage(ItemSlot slot)
        {
            DecreaseOrIncreaseText.text = "1";
            ItemSlot itemHero = default;

            if (slot.slotItem.Type == ItemType.Consumable)
                switch (slot.slotItem.consumables)
                {
                    case Consumables.TreatLife:
                        UsingHeal(playerCharacter, slot);
                        if (slot.itemCount == 0) PanelOptions.SetActive(false);
                        break;
                    case Consumables.RefillMana:
                        UsingMana(playerCharacter, slot);
                        if (slot.itemCount == 0) PanelOptions.SetActive(false);
                        break;
                    case Consumables.UsingHealPerSeconds:
                        StartCoroutine(UsingHealPerSeconds(playerCharacter, slot, slot.slotItem.Timer));
                        if (slot.itemCount == 0) PanelOptions.SetActive(false);
                        break;
                    case Consumables.UsingManaPerSeconds:
                        StartCoroutine(UsingEnergyPerSeconds(playerCharacter, slot, slot.slotItem.Timer));
                        if (slot.itemCount == 0) PanelOptions.SetActive(false);
                        break;
                }
            else
                switch (slot.slotItem.Type)
                {
                    case ItemType.Helmet:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Bib:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Boots:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Bijouterie:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Shoulders:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Mittens:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.Leggings:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.LeftHand:
                        DressingItem(itemHero, slot);
                        break;
                    case ItemType.RightHand:
                        DressingItem(itemHero, slot);
                        break;
                }
        }

        private void Separation()
        {
            DecreaseOrIncreaseText.text = "1";
            itemSettingsStatus = ItemSettingsStatus.Separation;
            StartCoroutine(DecreaseOrIncreaseValues());
            PanelOptionsSecondState.SetActive(true);
        }
        private void ThrowAway()
        {
            DecreaseOrIncreaseText.text = "1";
            itemSettingsStatus = ItemSettingsStatus.ThrowAway;
            StartCoroutine(DecreaseOrIncreaseValues());
            PanelOptionsSecondState.SetActive(true);
        }
        private void Delete()
        {
            DecreaseOrIncreaseText.text = "1";
            itemSettingsStatus = ItemSettingsStatus.Delete;
            StartCoroutine(DecreaseOrIncreaseValues());
            PanelOptionsSecondState.SetActive(true);
        }
        private void Accept(int NumberOfItems, ItemSlot slot)
        {
            switch (itemSettingsStatus)
            {
                case ItemSettingsStatus.Separation:
                    StopCoroutine(DecreaseOrIncreaseValues());
                    PanelOptions.SetActive(false);
                    PanelCloseOptions.SetActive(false);
                    SeparationItem(NumberOfItems);
                    break;

                case ItemSettingsStatus.ThrowAway:
                    StopCoroutine(DecreaseOrIncreaseValues());
                    slot.RemoveAndDrop(NumberOfItems, carrier.ItemDropPosition);
                    PanelOptions.SetActive(false);
                    PanelCloseOptions.SetActive(false);
                    break;

                case ItemSettingsStatus.Delete:
                    StopCoroutine(DecreaseOrIncreaseValues());
                    slot.Remove(NumberOfItems);
                    PanelOptions.SetActive(false);
                    PanelCloseOptions.SetActive(false);
                    break;
            }
        }

        private void SeparationItem(int numberOfItems)
        {
            bool hasEmptyCell = _slots.Any(slot => slot.slotItem == null);
            if (hasEmptyCell)
            {
                int emptyCellIndex = Enumerable.Range(0, _slots.Length).FirstOrDefault(j => _slots[j].slotItem == null);
                Utils.TransferItemQuantity(itemSlot, _slots[emptyCellIndex], numberOfItems);
            }
            else
            {
                UtilShowMessage.ShowMessagePanel.SetMessage("Инвентарь переполнен");
                UtilShowMessage.ShowMessagePanel.ShowMessagee();
            }
        }

        private void DressingItem(ItemSlot itemHero, ItemSlot slot)
        {
            itemHero = _slotsInHero.FirstOrDefault(item => item.GetItemType() == slot.slotItem.Type);
            if (itemHero != null)
            {
                slot.GetComponent<ItemSlotUIEvents>().DressedItem(itemHero, slot);
                slot.GetComponent<ItemSlotUIEvents>().CloseItemOptions.SetActive(false);
                PanelCloseOptions.SetActive(false);
                PanelOptions.SetActive(false);
            }
        }

        public void SetItemSlot(ItemSlot itemSlot)
        {
            this.itemSlot = itemSlot;
        }

        private IEnumerator DecreaseOrIncreaseValues()
        {
            while (true)
            {
                DecreaseOrIncreaseText.text = DecreaseOrIncreaseText.text.Replace("-", "");
                if (Convert.ToInt32(DecreaseOrIncreaseText.text) > itemSlot.itemCount)
                    DecreaseOrIncreaseText.text = itemSlot.itemCount.ToString();
                yield return null;
            }
        }

        private void AddValue()
        {
            int numberOfItem = Convert.ToInt32(DecreaseOrIncreaseText.text);
            numberOfItem++;
            if (numberOfItem > itemSlot.itemCount)
            {
                numberOfItem = itemSlot.itemCount;
                return;
            }
            DecreaseOrIncreaseText.text = numberOfItem.ToString();
        }

        private void SubtractValue()
        {
            int numberOfItem = Convert.ToInt32(DecreaseOrIncreaseText.text);
            if (numberOfItem <= 1) return;
            numberOfItem--;
            DecreaseOrIncreaseText.text = numberOfItem.ToString();
        }

        public ItemSlot[] GetItemSlots()
        {
            return _slots;
        }
    }
}