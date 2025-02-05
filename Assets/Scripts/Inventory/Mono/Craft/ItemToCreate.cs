using UnityEngine;
using Inventory;
using UnityEngine.UI;

namespace Craft
{
    //Вобор предмета для создания
    public class ItemToCreate : MonoBehaviour
    {
        public Item item;
        public Image _imageItem;

        public CraftAndAmount craftAndAmount;
        CraftItem craftItem;
        void Start()
        {
            craftItem = GameObject.Find("Panel.Creation").GetComponent<CraftItem>();
        }
        public void AcceptValue(Item item, GameObject CraftItemRightPanel, GameObject ConsumablesList, GameObject PrefabConsumable)
        {
            _imageItem.sprite = item.Icon;
            this.item = item;

            gameObject.GetComponent<Button>().onClick.AddListener(() => ChooseAnItemToCraft(ConsumablesList, PrefabConsumable));

            craftAndAmount = CraftItemRightPanel.GetComponent<CraftAndAmount>();
        }

        public void ChooseAnItemToCraft(GameObject ConsumablesList, GameObject PrefabConsumable)
        {
            craftItem.AmountText.text = "1";
            craftItem.ItemSelected = item;
            //Очистка списка компонентов
            foreach (Transform Component in ConsumablesList.transform)
            {
                Destroy(Component.gameObject);
            }
            //Применение значений для предмета который хочу создать
            craftAndAmount.AcceptValueCraftItem(item);
            //Заполнение списка компонентами
            for (int AmountConsumables = 0; AmountConsumables < item.craft.Length; AmountConsumables++)
            {
                GameObject Consumable = Instantiate(PrefabConsumable, ConsumablesList.transform);
                Consumable.GetComponent<Material>().AcceptValue
                    (item.craft[AmountConsumables].WhichSubjects, item.craft[AmountConsumables].WhichSubjects.Icon, item.craft[AmountConsumables].Amount.ToString());
            }

            foreach (Transform child in ConsumablesList.transform)
            {
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
        int ChangeColor(Item item)
        {
            int countItem = 0;
            foreach (ItemSlot itemSlot in craftItem.itemContainer.GetItemSlots())
            {
                if (item == itemSlot.slotItem)
                {
                    countItem += itemSlot.itemCount;
                }
            }
            return countItem;
        }
    }
}