using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using Enemy;
using Attribute;
namespace DebuffBuff
{
    public abstract class Debuff : MonoBehaviour
    {
        public static void EnemyDamageOneShot(float[] typeResistance, int[] indices, float[] damages, PlayerCharacter playerCharecter = null, EnemyCharacter enemyCharacter = null)
        {
            for (int count = 0; count < indices.Length; count++)
            {
                int index = indices[count];
                if (index >= 0 && index < typeResistance.Length)
                {
                    float different = 1f - (typeResistance[index] / 100);
                    if (playerCharecter != null)
                    {
                        playerCharecter.SetHealthPlus(-(different * damages[count]));
                    }
                    if (enemyCharacter != null)
                    {
                        enemyCharacter.SetHealthMinus((different * damages[count]));
                    }
                }
            }
        }
        public static IEnumerator DOT(float[] damageDota, float[] typeResistance, int[] indices, float Time, GameObject prefabAtribut, PlayerCharacter playerCharecter = null, EnemyCharacter enemyCharacter = null)
        {
            GameObject atribut = default;
            Atribute atribute = default;
            if (playerCharecter != null)
            {
                atribut = Instantiate(prefabAtribut, playerCharecter.panelAtribute.transform);
                atribute = atribut.GetComponent<Atribute>();
            }
            for (int timer = 0; timer <= Time; Time--)
            {
                for (int count = 0; count < indices.Length; count++)
                {
                    int index = indices[count];
                    if (index >= 0 && index < typeResistance.Length)
                    {
                        float different = 1f - (typeResistance[index] / 100);
                        if (playerCharecter != null)
                        {
                            atribute.SetTimerValue(Time);
                            if (timer == Time) Destroy(atribut);
                            playerCharecter.SetHealthPlus(-(different * damageDota[index]));
                        }
                        if (enemyCharacter != null)
                            enemyCharacter.SetHealthMinus(12);
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }

        public static IEnumerator DebuffPerSecond(float[] typeResistance, int[] indices, float[] newValue, float time, GameObject prefabAtribut = null, PlayerCharacter playerCharacter = null)
        {
            GameObject atribut = default;
            Atribute atribute = default;
            if (playerCharacter != null)
            {
                atribut = Instantiate(prefabAtribut, playerCharacter.panelAtribute.transform);
                atribute = atribut.GetComponent<Atribute>();
            }
            for (int count = 0; count < indices.Length; count++)
            {
                int index = indices[count];
                if (index >= 0 && index < typeResistance.Length)
                {
                    typeResistance[index] -= newValue[count];
                }
            }
            //for (int time = 1; time <= timer; timer--)
            for (int timer = 1; timer <= time; time--)
            {
                if (playerCharacter != null)
                    atribute.SetTimerValue(time);
                yield return new WaitForSeconds(1);
            }
            for (int count = 0; count < indices.Length; count++)
            {
                int index = indices[count];
                if (index >= 0 && index < typeResistance.Length)
                {
                    typeResistance[index] += newValue[count];
                }
            }
        }
    }
}