using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Inventory
{
    // Класс который хранит в себе все механики расходников
    public abstract class ItemManager : MonoBehaviour
    {
        // одноразовое применения лечения
        public void UsingHeal(PlayerCharacter playerCharacter, ItemSlot item)
        {
            var result = playerCharacter.GetHealth() + item.slotItem.HealthOrPerSecond;

            if (result <= playerCharacter.GetMaxHealth())
                playerCharacter.SetHealth(result);
            else
                playerCharacter.SetHealth(playerCharacter.GetMaxHealth());
            item.Remove();
        }
        // одноразовое пополнение маны
        public void UsingMana(PlayerCharacter playerCharacter, ItemSlot item)
        {
            var result = playerCharacter.GetEnergy() + item.slotItem.EnergyOrPerSecond;

            if (result <= playerCharacter.GetMaxEnergy())
                playerCharacter.SetEnergy(result);
            else
                playerCharacter.SetEnergy(playerCharacter.GetMaxEnergy());
            item.Remove();
        }
        // пополнение здоровья со временем
        public IEnumerator UsingHealPerSeconds(PlayerCharacter playerCharacter, ItemSlot item, float timer)
        {
            ItemSlot itemSlot = new ItemSlot
            {
                slotItem = item.slotItem
            };
            item.Remove();

            while (0 < timer)
            {
                var result = playerCharacter.GetHealth() + itemSlot.slotItem.HealthOrPerSecond;

                if (result <= playerCharacter.GetMaxHealth())
                    playerCharacter.SetHealth(result);
                else
                    playerCharacter.SetHealth(playerCharacter.GetMaxHealth());

                yield return new WaitForSeconds(1f);

                timer -= 1f;
            }
        }
        // пополнение маны со временем
        public IEnumerator UsingEnergyPerSeconds(PlayerCharacter playerCharacter, ItemSlot item, float timer)
        {
            ItemSlot itemSlot = new ItemSlot
            {
                slotItem = item.slotItem
            };
            item.Remove();

            while (0 < timer)
            {
                var result = playerCharacter.GetEnergy() + itemSlot.slotItem.EnergyOrPerSecond;

                if (result <= playerCharacter.GetMaxEnergy())
                    playerCharacter.SetEnergy(result);
                else
                    playerCharacter.SetEnergy(playerCharacter.GetMaxEnergy());

                yield return new WaitForSeconds(1f);

                timer -= 1f;
            }
        }
    }
}