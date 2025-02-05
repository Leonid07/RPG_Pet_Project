using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Inventory
{
    // ����� ������� ������ � ���� ��� �������� �����������
    public abstract class ItemManager : MonoBehaviour
    {
        // ����������� ���������� �������
        public void UsingHeal(PlayerCharacter playerCharacter, ItemSlot item)
        {
            var result = playerCharacter.GetHealth() + item.slotItem.HealthOrPerSecond;

            if (result <= playerCharacter.GetMaxHealth())
                playerCharacter.SetHealth(result);
            else
                playerCharacter.SetHealth(playerCharacter.GetMaxHealth());
            item.Remove();
        }
        // ����������� ���������� ����
        public void UsingMana(PlayerCharacter playerCharacter, ItemSlot item)
        {
            var result = playerCharacter.GetEnergy() + item.slotItem.EnergyOrPerSecond;

            if (result <= playerCharacter.GetMaxEnergy())
                playerCharacter.SetEnergy(result);
            else
                playerCharacter.SetEnergy(playerCharacter.GetMaxEnergy());
            item.Remove();
        }
        // ���������� �������� �� ��������
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
        // ���������� ���� �� ��������
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