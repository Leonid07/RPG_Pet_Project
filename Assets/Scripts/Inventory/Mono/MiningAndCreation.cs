using UnityEngine;
using UnityEngine.UI;
using System;
using Radar;

namespace Inventory
{
    public class MiningAndCreation : MonoBehaviour
    {
        [SerializeField] float HP;
        private MapObject mapObject;
        public HarvestDrop[] harvestDrops;

        public bool CollectiveResources = false;

        [Tooltip("Этот объект создаётся после того как ресурс был собран")]
        public GameObject IsCollected;

        [Tooltip("Название объекта которое будет показываться в выплывающем сообщении")]
        public string Name;

        private Rigidbody rb;
        private ItemContainer itemContainer;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            mapObject = GetComponent<MapObject>();
        }
        private void Harvest()
        {
            System.Random prng = new System.Random(GetHashCode());
            for (int i = 0; i < harvestDrops.Length; i++)
            {
                HarvestDrop drop = harvestDrops[i];
                for (int count = 0; count < drop.count; count++)
                {
                    if (prng.NextDouble() <= drop.chance)
                    {
                        if (!itemContainer.SearchForAnEmptyCell())
                        {
                            Utils.InstantiateItemCollector(drop.itemToDrop, gameObject.transform.position);
                        }
                        else
                        {
                            itemContainer.AddItem(drop.itemToDrop);
                        }
                    }
                }
            }
        }
        public void CollectResurce(ItemContainer itemContainer)
        {
            if (CollectiveResources)
            {
                System.Random prng = new System.Random(GetHashCode());
                for (int i = 0; i < harvestDrops.Length; i++)
                {
                    HarvestDrop drop = harvestDrops[i];
                    for (int count = 0; count < drop.count; count++)
                    {
                        if (prng.NextDouble() <= drop.chance)
                        {
                            if (!itemContainer.SearchForAnEmptyCell())
                            {
                                Utils.InstantiateItemCollector(drop.itemToDrop, gameObject.transform.position);
                            }
                            else
                            {
                                itemContainer.AddItem(drop.itemToDrop);
                            }
                        }
                    }
                }
                Instantiate(IsCollected, transform.position, transform.rotation);
                Destroy(mapObject.mapMarker);
                Destroy(gameObject);
            }
        }
        public void SetMinusHP(int Hp, ItemContainer itemContainer)
        {
            if (!CollectiveResources)
            {
                HP -= Hp;
                this.itemContainer = itemContainer;
                if (HP <= 0)
                {
                    Harvest();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    Destroy(mapObject.mapMarker);
                    Destroy(gameObject, 5);
                }
            }
        }
    }

    [System.Serializable]
    public struct HarvestDrop
    {
        public Item itemToDrop;
        [Range(0, 1)]
        public float chance;
        public int count; //Количество предметов которые могут выпасть
    }
}
