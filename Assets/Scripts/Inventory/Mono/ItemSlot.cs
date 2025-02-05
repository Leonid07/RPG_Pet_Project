using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    // Класс предмета в инвенторе
    public class ItemSlot : MonoBehaviour
    {
        public Item slotItem;
        public int itemCount;
        public bool isDressed = false;
        [SerializeField] private ItemType itemType;

        public bool IsEmpty { get { return itemCount <= 0; } }
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject _imageCountItem;
        [SerializeField] private Sprite defoultSprite;
        [SerializeField] private TMP_Text countText;

        private void Awake()
        {
            if (countText != null)
            {
                countText.text = string.Empty;
                _imageCountItem.SetActive(false);
            }
        }

        public bool Add(Item item)
        {
            if (IsAddable(item))
            {
                slotItem = item;
                itemCount++;
                OnSlotModified();
                return true;
            }
            else return false;

        }

        public void RemoveAndDrop(int amount, Vector3 dropPosition)
        {
            for (int i = 0; i < amount; i++)
            {
                if (itemCount > 0)
                {
                    Utils.InstantiateItemCollector(slotItem, dropPosition);
                    itemCount--;
                }
                else break;
            }
            OnSlotModified();
        }

        public void Remove(int amount = 1)
        {
            itemCount -= amount > itemCount ? itemCount : amount;
            OnSlotModified();
        }

        public void Clear()
        {
            itemCount = 0;
            OnSlotModified();
        }

        public void ClearAndDrop(Vector3 dropPosition)
        {
            RemoveAndDrop(itemCount, dropPosition);
        }

        private bool IsAddable(Item item)
        {
            if (item != null)
            {
                if (IsEmpty) return true;
                else
                {
                    if (item == slotItem && itemCount < item.ItemPerSlot) return true;
                    else return false;
                }
            }
            return false;
        }

        private void OnSlotModified()
        {
            if (!IsEmpty)
            {
                iconImage.sprite = slotItem.Icon;
                if (countText != null)
                {
                    countText.text = itemCount.ToString();
                    _imageCountItem.SetActive(itemCount > 1);
                }
            }
            else
            {
                itemCount = 0;
                slotItem = null;
                iconImage.sprite = defoultSprite;
                if (countText != null)
                {
                    countText.text = string.Empty;
                    _imageCountItem.SetActive(false);
                }
            }
        }
        public ItemType GetItemType()
        {
            return itemType;
        }
    }
}