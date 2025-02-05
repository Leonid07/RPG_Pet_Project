using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

namespace Craft
{
    //����� ��� ���������� ���������� ��� ������
    public class CraftItem : MonoBehaviour
    {
        public ItemContainer itemContainer;

        public GameObject LeftPanel;
        public GameObject PrefabItemCraft;
        public List<Item> ItemCraft;
        public List<ItemToCreate> ListItemToCreate;
        public Item ItemSelected;

        public GameObject CraftItemRightPanel;
        public GameObject ConsumablesList;
        public GameObject PrefabConsumable;

        public Button PlusAmountButton;
        public Button MinusAmountButton;
        public Button CreateButton;

        public TMP_InputField AmountText;

        void Start()
        {
            foreach (var Item in ItemCraft)
            {
                GameObject newItemCraft = Instantiate(PrefabItemCraft, LeftPanel.transform);
                newItemCraft.GetComponent<ItemToCreate>().AcceptValue(Item, CraftItemRightPanel, ConsumablesList, PrefabConsumable);
                ListItemToCreate.Add(newItemCraft.GetComponent<ItemToCreate>());
            }
            PlusAmountButton.onClick.AddListener(PlusAmount);
            MinusAmountButton.onClick.AddListener(MinusAmount);
            CreateButton.onClick.AddListener(Create);
            AmountText.onValueChanged.AddListener(OnInputValueChanged);
        }
        int ChangeColor(Item item)
        {
            int countItem = 0;
            foreach (ItemSlot itemSlot in itemContainer.GetItemSlots())
            {
                if (item == itemSlot.slotItem)
                {
                    countItem += itemSlot.itemCount;
                }
            }
            return countItem;
        }
        void PlusAmount()
        {
            int amount = Convert.ToInt32(AmountText.text);
            amount++;
            AmountText.text = amount.ToString();

            foreach (Transform child in ConsumablesList.transform)
            {
                int index = child.GetSiblingIndex();
                child.GetComponent<Material>().SetCountConsumable(child.GetComponent<Material>().GetCountConsumable() + ItemSelected.craft[index].Amount);
                if(ChangeColor(child.GetComponent<Material>().item) >= child.GetComponent<Material>().GetCountConsumable())
                {
                    child.GetComponent<Material>().GetTMP_TextConsumable().color = Color.white;
                }
                else
                {
                    child.GetComponent<Material>().GetTMP_TextConsumable().color = Color.red;
                }
            }
        }
        void MinusAmount()
        {
            int amount = Convert.ToInt32(AmountText.text);
            amount--;
            if (amount <= 0) return;
            AmountText.text = amount.ToString();

            foreach (Transform child in ConsumablesList.transform)
            {
                int index = child.GetSiblingIndex();
                child.GetComponent<Material>().SetCountConsumable(child.GetComponent<Material>().GetCountConsumable() - ItemSelected.craft[index].Amount);
                if (ChangeColor(child.GetComponent<Material>().item) >= child.GetComponent<Material>().GetCountConsumable())
                {
                    child.GetComponent<Material>().GetTMP_TextConsumable().color = Color.white;
                }
                else
                {
                    child.GetComponent<Material>().GetTMP_TextConsumable().color = Color.red;
                }
            }
        }
        void Create()
        {
            List<Material> Material = new List<Material>();
            foreach (Transform consumable in ConsumablesList.transform)
            {
                Material component = consumable.GetComponent<Material>();
                if (component != null)
                    Material.Add(component);
            }
            // ��� �������� ������� �� ���������� �������� ��� �������� ������ ��������� ���������
            if (Cheack(itemContainer.GetItemSlots(), Material))
            {
                // ���� ������� ���������� ����� ��������� ����� ��� �������� ��������
                for (int countMaterial = 0; countMaterial < Material.Count; countMaterial++)
                {
                    // ���� ������� ���������� ����� ���������� ������ �������� ������� ��������� ��� �������� ��������
                    for (int amount = 0; amount < Material[countMaterial].GetCountConsumable(); amount++)
                    {
                        // ���� ������� ������� �������������� �������� � ��������� � ����� � ������� ���� �� ��������� �� ���������� ���� ��������
                        for (int item = 0; item < itemContainer.GetItemSlots().Length; item++)
                        {
                            if (itemContainer.GetItemSlots()[item].slotItem == Material[countMaterial].item)
                            {
                                itemContainer.GetItemSlots()[item].Remove();
                                break;
                            }
                        }
                    }
                }
                // ���� ������� ���������� ������� ��������� ����� ������� �������� ������ �� AmountText
                for (int amountItem = 0; amountItem < Convert.ToInt32(AmountText.text); amountItem++)
                {
                    itemContainer.AddItem(CraftItemRightPanel.GetComponent<CraftAndAmount>().GetItem());
                }
            }
        }

        public bool Cheack(ItemSlot[] item, List<Material> Material)
        {
            int[] count = new int[Material.Count];
            int countResourses = 0;
            for (int countMaterial = 0; countMaterial < Material.Count; countMaterial++)
            {
                for (int itemCount = 0; itemCount < item.Length; itemCount++)
                {
                    if (item[itemCount].slotItem == Material[countMaterial].item)
                    {
                        count[countMaterial] += item[itemCount].itemCount;
                        if (Material[countMaterial].GetCountConsumable() <= count[countMaterial])
                        {
                            countResourses++;
                            if (countResourses == Material.Count)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        void OnInputValueChanged(string newValue)
        {
            // �������� ������������� ��������� �������� � �����
            if (int.TryParse(newValue, out int value))
            {
                // ���� �������� ������ 1, ������������� ��� ������ 1
                if (value < 1)
                {
                    AmountText.text = "1";
                }
            }
            else
            {
                // ���� ��������� �������� �� ������� ������������� � �����, ���������� ���� � �������� �� ��������� (��������, 1)
                AmountText.text = "1";
            }
        }
    }
}