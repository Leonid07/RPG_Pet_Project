using UnityEngine;

namespace Inventory
{
    //Ѕазовый класс дл€ всех объектов, с которыми может взаимодействовать интерактор.
    public abstract class Interactable : MonoBehaviour
    {
        //Ётот метод вызываетс€, когда интерактор пытаетс€ взаимодействовать с объектом.
        //ѕереданный в Interactor Ч это интерактор, который пытаетс€ взаимодействовать.
        public virtual void OnInteract(Interactor interactor)
        {
            Debug.Log("Interacting...");
        }
    }
}
