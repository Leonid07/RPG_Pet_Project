using UnityEngine;

namespace Inventory
{
    //���� ������ ������������� � ��������-��������� ���������.
    //���������� ��������� � ��� ��������� ������� ��������� � �����, ������� ���������� ����� ��������� ��� ������������.
    public class ItemCollector : MonoBehaviour
    {
        [SerializeField] Item item;

        //����� ItemCollector ���������� � �������, ������� ������� ���� ����� � �������� ������� ����� ��������.
        public void Create(Item item)
        {
            this.item = item;
        }
        public Item GetItem()
        {
            return item;
        }

        //��� ������������ ����������� ����� ����������� ������� �������� ������� ����� ������������� � ��������� �����������.
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "ItemCollectionArea")
            {
                other.GetComponent<ItemCollectionArea>().interactor.AddToInventory(item, gameObject);
            }
        }
    }
}
