using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Inventory;
namespace Craft
{
    public class Material : MonoBehaviour
    {
        public Item item;
        public Image ImageConsumable;
        public TMP_Text AmountConsumable;

        public void AcceptValue(Item item, Sprite ImageConsumable, string AmountConsumable)
        {
            this.item = item;
            this.ImageConsumable.sprite = ImageConsumable;
            this.AmountConsumable.text = AmountConsumable;
        }
        public int GetCountConsumable()
        {
            return Convert.ToInt32(AmountConsumable.text);
        }
        public TMP_Text GetTMP_TextConsumable()
        {
            return AmountConsumable;
        }
        public void SetCountConsumable(int Count)
        {
            AmountConsumable.text = Count.ToString();
        }
    }
}