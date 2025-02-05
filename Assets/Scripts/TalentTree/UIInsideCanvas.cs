using System.Collections;
using UnityEngine;
public class UIInsideCanvas : MonoBehaviour
{
    public RectTransform canvasRectTransform;
    private RectTransform imageRectTransform;

    private void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    public void PanelInsideCanvas()
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