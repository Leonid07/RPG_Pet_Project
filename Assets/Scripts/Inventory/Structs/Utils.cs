using DG.Tweening;
using Player;
using System.Collections;
using UnityEngine;
using TMPro;
namespace Inventory
{
    public readonly struct Utils
    {
        public static void TransferItem(ItemSlot trigger, ItemSlot target)
        {
            if (trigger == target) return;

            Item triggerItem = trigger.slotItem;
            Item targetItem = target.slotItem;

            int triggerItemCount = trigger.itemCount;

            if (!trigger.IsEmpty)
            {
                if (target.IsEmpty || targetItem == triggerItem)
                {
                    for (int i = 0; i < triggerItemCount; i++)
                    {
                        if (target.Add(triggerItem)) trigger.Remove();
                        else return;
                    }
                }
                else
                {
                    int targetItemCount = target.itemCount;

                    target.Clear();
                    for (int i = 0; i < triggerItemCount; i++) target.Add(triggerItem);

                    trigger.Clear();
                    for (int i = 0; i < targetItemCount; i++) trigger.Add(targetItem);
                }
            }
        }

        //Этот метод пытается передать переданное количество элементов из триггерного ItemSlot в целевой ItemSlot.
        public static void TransferItemQuantity(ItemSlot trigger, ItemSlot target, int amount)
        {
            if (!trigger.IsEmpty)
            {
                for (int i = 0; i < amount; i++)
                {
                    if (!trigger.IsEmpty)
                    {
                        if (target.Add(trigger.slotItem)) trigger.Remove();
                        else return;
                    }
                    else return;
                }
            }
        }

        public static void AnimateObjectDisappear(GameObject objToAnimate, float durationInSeconds, bool destroyAfterAnimation = true)
        {
            if (objToAnimate != null)
            {
                // Используем DOTween для изменения масштаба объекта до нулевого масштаба в заданной длительности
                objToAnimate.transform.DOScale(Vector3.zero, durationInSeconds)
                    .OnComplete(() =>
                    {
                        if (destroyAfterAnimation)
                        {
                            GameObject.Destroy(objToAnimate);
                        }
                        else
                        {
                            objToAnimate.SetActive(false);
                        }
                    });
            }
        }

        public static void InstantiateItemCollector(Item item, Vector3 position)
        {
            GameObject inst = Object.Instantiate(item.Prefab, position, Quaternion.Euler(0f, 0, 45f));
            inst.GetComponent<Collider>().enabled = true;
            inst.GetComponent<Rigidbody>().useGravity = true;
            inst.AddComponent<ItemCollector>().Create(item);
        }

        public static void HighlightObject(GameObject obj, Color highlightColor)
        {
            obj.GetComponent<MeshRenderer>().material.color = highlightColor;
        }

        public static void UnhighlightObject(GameObject obj, Color original)
        {
            obj.GetComponent<MeshRenderer>().material.color = original;
        }

        public static void UnhighlightObject(GameObject obj)
        {
            UnhighlightObject(obj, Color.white);
        }
        public static void ShowAndHidePanel(Transform transformPanel)
        {
            transformPanel.localPosition = (transformPanel.localPosition.x != 0) ? Vector3.zero : new Vector3(10000, 10000, 10000);
        }
        public static void ShowPanel(Transform transformPanel)
        {
            transformPanel.localPosition = Vector3.zero;
        }
        public static void HidePanel(Transform transformPanel)
        {
            transformPanel.localPosition = new Vector3(10000, 10000, 10000);
        }
    }
}
