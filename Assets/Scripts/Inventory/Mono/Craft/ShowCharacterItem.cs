using UnityEngine;
using UnityEngine.EventSystems;
using Inventory;

namespace Craft
{
    public class ShowCharacterItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        GameObject panelCharacter;
        PanelInsideCanvas panelInsideCanvas;

        ItemToCreate itemToCreate;

        private void Start()
        {
            panelCharacter = GameObject.Find("Panel.ItemCharacterCreation");
            panelInsideCanvas = panelCharacter.GetComponent<PanelInsideCanvas>();
            itemToCreate = GetComponent<ItemToCreate>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panelCharacter.SetActive(true);
            panelInsideCanvas.PanelCharacterItem(itemToCreate.item);
            panelCharacter.transform.position = Input.mousePosition;
            panelInsideCanvas.PanelInside_Canvas();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panelCharacter.SetActive(false);
        }
    }
}