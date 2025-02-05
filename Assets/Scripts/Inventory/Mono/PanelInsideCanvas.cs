using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PanelInsideCanvas : MonoBehaviour
    {
        public ItemContainer itemContainer;

        public Image icon;
        public TMP_Text nameItem;
        public TMP_Text description;
        public RectTransform canvasRectTransform;

        private Vector2 positionPanelRectTransform;
        private Vector2 heightPanelRectTrancform;
        private RectTransform imageRectTransform;
        private void Start()
        {
            imageRectTransform = GetComponent<RectTransform>();
            positionPanelRectTransform = imageRectTransform.anchoredPosition;
            heightPanelRectTrancform = imageRectTransform.sizeDelta;
        }

        public void PanelCharacterItem(Item item)
        {
            icon.sprite = item.Icon;
            nameItem.text = item.ItemName;
            switch (item.Type)
            {
                case ItemType.Bib:
                case ItemType.Bijouterie:
                case ItemType.LeftHand:
                case ItemType.Helmet:
                case ItemType.Shoulders:
                case ItemType.Mittens:
                case ItemType.Leggings:
                case ItemType.Boots:
                    description.text = item.Discription + "\nТип защиты";
                    for (int resistanceCount = 0; resistanceCount < item._Resistance.Length; resistanceCount++)
                    {
                        if (item._Resistance[resistanceCount] > 0)
                        {
                            description.text += $"\n{item._stringResistancee[resistanceCount]} {item._Resistance[resistanceCount]}";
                        }
                    }
                    break;
                case ItemType.RightHand:
                    description.text = item.Discription + "\nТип урона";
                    for (int resistanceCount = 0; resistanceCount < item._TypeDamage.Length; resistanceCount++)
                    {
                        if (item._TypeDamage[resistanceCount] > 0)
                        {
                            description.text += $"\n{item._stringResistancee[resistanceCount]} {item._TypeDamage[resistanceCount]}";
                        }
                    }
                    break;
                case ItemType.Consumable:
                    description.text = item.Discription;
                    break;
                case ItemType.Material:
                    description.text = item.Discription;
                    break;
            }
            AutoExpandPanel(description, imageRectTransform);
        }
        public void AutoExpandPanel(TMP_Text text, RectTransform panel)
        {
            text.ForceMeshUpdate();
            float lineHeight = (text.fontSize * text.textInfo.lineCount) + (2.3f * (text.textInfo.lineCount - 1));
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, heightPanelRectTrancform.y + lineHeight);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, positionPanelRectTransform.y - lineHeight);
        }
        public void PanelInside_Canvas()
        {
            Vector3[] canvasCorners = new Vector3[4];
            Vector3[] imageCorners = new Vector3[4];

            canvasRectTransform.GetWorldCorners(canvasCorners);
            imageRectTransform.GetWorldCorners(imageCorners);

            float minX = canvasCorners[0].x;
            float minY = canvasCorners[0].y;

            float maxX = canvasCorners[2].x;
            float maxY = canvasCorners[2].y;

            // Получаем текущую позицию изображения
            Vector3 currentPosition = imageRectTransform.position;

            // Проверяем и корректируем позицию изображения, чтобы оно не выходило за пределы Canvas
            float clampedX = Mathf.Clamp(currentPosition.x, minX + (imageRectTransform.rect.width / 2), maxX - (imageRectTransform.rect.width / 2));
            float clampedY = Mathf.Clamp(currentPosition.y, minY + (imageRectTransform.rect.height / 2), maxY - (imageRectTransform.rect.height / 2));

            // Применяем скорректированную позицию к изображению
            imageRectTransform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }
}