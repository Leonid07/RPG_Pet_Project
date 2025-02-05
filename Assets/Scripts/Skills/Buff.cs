using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemy;
using Attribute;

namespace DebuffBuff
{
    public abstract class Buff : MonoBehaviour
    {
        public static void Heal(float amountHealth, PlayerCharacter playerCharacter = null, EnemyCharacter enemyCharacter = null)
        {
            if (playerCharacter != null)
                playerCharacter.SetHealthPlus(amountHealth);
            if (enemyCharacter != null)
                enemyCharacter.SetHealthPlus(amountHealth);
        }
        public static IEnumerator HealPerSecond(float amountHealth, int timer, PlayerCharacter playerCharacter = null, GameObject prefabAtribute = null, EnemyCharacter enemyCharacter = null)
        {
            GameObject atribut = Instantiate(prefabAtribute, playerCharacter.panelAtribute.transform);
            while (0 < timer)
            {
                if (playerCharacter != null)
                {
                    Atribute atribute = atribut.GetComponent<Atribute>();

                    var result = playerCharacter.GetHealth() + amountHealth;
                    atribute.SetTimerValue(timer);
                    if (result <= playerCharacter.GetMaxHealth())
                        playerCharacter.SetHealth(result);
                    else
                        playerCharacter.SetHealth(playerCharacter.GetMaxHealth());
                }
                if (enemyCharacter != null)
                {
                    var result = enemyCharacter.GetHealth() + amountHealth;
                    if (result <= enemyCharacter.GetMaxHealth())
                        enemyCharacter.SetHealth(result);
                    else
                        enemyCharacter.SetHealth(enemyCharacter.GetMaxHealth());
                }
                yield return new WaitForSeconds(1);
                timer--;
            }
            Destroy(atribut);
        }
        public static IEnumerator RaiseResistanceForTime(int[] indices, float[] newValue, float timer, PlayerCharacter playerCharacter = null, GameObject prefubAtribute = null, EnemyCharacter enemyCharacter = null)
        {
            if (playerCharacter != null)
            {
                GameObject atribut = Instantiate(prefubAtribute, playerCharacter.panelAtribute.transform);
                Atribute atribute = atribut.GetComponent<Atribute>();
                for (int count = 0; count < indices.Length; count++)
                {
                    int index = indices[count];
                    if (index >= 0 && index < playerCharacter.GetResistance().Length)
                    {
                        playerCharacter.GetResistance()[index] += newValue[count];
                    }
                }
                for (int time = 1; time <= timer; timer--)
                {
                    atribute.SetTimerValue(timer);
                    yield return new WaitForSeconds(1);
                }
                Destroy(atribut);
                for (int count = 0; count < indices.Length; count++)
                {
                    int index = indices[count];
                    if (index >= 0 && index < playerCharacter.GetResistance().Length)
                    {
                        playerCharacter.GetResistance()[index] -= newValue[count];
                    }
                }
            }
            if (enemyCharacter != null)
            {
                for (int count = 0; count < indices.Length; count++)
                {
                    int index = indices[count];
                    if (index >= 0 && index < enemyCharacter.GetResistance().Length)
                    {
                        enemyCharacter.GetResistance()[index] += newValue[count];
                    }
                }
                yield return new WaitForSeconds(timer);
                for (int count = 0; count < indices.Length; count++)
                {
                    int index = indices[count];
                    if (index >= 0 && index < enemyCharacter.GetResistance().Length)
                    {
                        enemyCharacter.GetResistance()[index] -= newValue[count];
                    }
                }
            }
        }
        public static IEnumerator RaiseDamageForTime(float damageInPercent, int timer, PlayerCharacter playerCharacter = null, GameObject prefabAtribute = null, EnemyCharacter enemyCharacter = null)
        {
            if (playerCharacter != null)
            {
                GameObject atribut = Instantiate(prefabAtribute, playerCharacter.panelAtribute.transform);
                Atribute atribute = atribut.GetComponent<Atribute>();

                var trueDamage = playerCharacter.GetDamage();
                playerCharacter.SetDamage(playerCharacter.GetDamage() * (1 + damageInPercent / 100));
                for (int time = 1; time <= timer; timer--)
                {
                    atribute.SetTimerValue(timer);
                    yield return new WaitForSeconds(1);
                }
                Destroy(atribut);
                playerCharacter.SetDamage(trueDamage);
            }
            //if (enemyCharacter != null)
            //{
            //    var trueDamage = enemyCharacter.GetDamage();
            //    enemyCharacter.SetDamage(enemyCharacter.GetDamage() * (1 + damageInPercent / 100));
            //    yield return new WaitForSeconds(timer);
            //    enemyCharacter.SetDamage(trueDamage);
            //}
            if (enemyCharacter != null)
            {
                var trueDamage = enemyCharacter.Damage;
                enemyCharacter.Damage = enemyCharacter.Damage * (1 + damageInPercent / 100);
                yield return new WaitForSeconds(timer);
                enemyCharacter.Damage = trueDamage;
            }
        }
    }
}