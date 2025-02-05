using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Attribute
{
    public class Atribute : MonoBehaviour
    {
        [SerializeField] Image imageAtribute;
        [SerializeField] TMP_Text timer;

        public Sprite GetImageAtribute()
        {
            return imageAtribute.sprite;
        }
        public int GetTimer()
        {
            return Convert.ToInt32(timer.text);
        }
        public void SetTimerValue(float time)
        {
            timer.text = time.ToString();
        }
    }
}