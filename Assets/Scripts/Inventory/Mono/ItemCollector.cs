using UnityEngine;

namespace Inventory
{
    //Этот скрипт прикрепляется к объектам-сборщикам предметов.
    //Коллекторы предметов — это плавающие префабы предметов в сцене, которые интерактор может подобрать при столкновении.
    public class ItemCollector : MonoBehaviour
    {
        [SerializeField] Item item;

        //Когда ItemCollector прикреплен к объекту, следует вызвать этот метод и передать элемент этому сборщику.
        public void Create(Item item)
        {
            this.item = item;
        }
        public Item GetItem()
        {
            return item;
        }

        //При срабатывании интерактора будет предпринята попытка добавить предмет этого коллекционера в инвентарь интерактора.
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "ItemCollectionArea")
            {
                other.GetComponent<ItemCollectionArea>().interactor.AddToInventory(item, gameObject);
            }
        }
    }
}
