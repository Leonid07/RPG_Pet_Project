using UnityEngine;

namespace Inventory
{
    //������� ����� ��� ���� ��������, � �������� ����� ����������������� ����������.
    public abstract class Interactable : MonoBehaviour
    {
        //���� ����� ����������, ����� ���������� �������� ����������������� � ��������.
        //���������� � Interactor � ��� ����������, ������� �������� �����������������.
        public virtual void OnInteract(Interactor interactor)
        {
            Debug.Log("Interacting...");
        }
    }
}
