using UnityEngine;
using UnityEngine.UI;

namespace Radar
{
    public class MapMarker : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private Image image = null;

        [HideInInspector] public MapMarkerData Data;
        [HideInInspector] public MapObject Object;
        [HideInInspector] public GameObject GO;

        public void Initialize(MapObject obj, GameObject go, float size)
        {
            Data = obj.Data;
            Object = obj;
            GO = go;
            image.sprite = Data.Icon;
            if (Data.Icon == null)
                image.enabled = false;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }
        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }
    }
}