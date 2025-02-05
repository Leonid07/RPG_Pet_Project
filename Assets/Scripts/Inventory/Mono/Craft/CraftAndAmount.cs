using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using UnityEngine.UI;

namespace Craft
{
    //���������� ����� ������� ����� ����������� � ������ ������
    public class CraftAndAmount : MonoBehaviour
    {
        public Item item;
        public Image ImageItem;

        public Item GetItem()
        {
            return item;
        }
        public void AcceptValueCraftItem(Item item)
        {
            this.item = item;
            ImageItem.sprite = item.Icon;
        }
    }
}