using System.Collections.Generic;
using UnityEngine;

namespace Radar
{
    public class Map : MonoBehaviour
    {
        [SerializeField] RectTransform rectTransform = null;
        [SerializeField] RectTransform markersTransform = null;

        [Header("Ссылки на объекты")]
        public MapObject Player;
        public List<MapObject> Objects = new List<MapObject>();
        public List<MapObject> StaticObjects = new List<MapObject>();
        public MapMarker MarkerPrefab;
        public MapMarker ReferenceMarker;
        public List<MapMarker> Markers { get; set; } = new List<MapMarker>();
        public List<MapMarker> StaticMarkers { get; set; } = new List<MapMarker>();

        [Header("Переменные карты")]
        public Vector2 WorldSize = new Vector2(30, 30);
        public float MarkerSize = 15;
        [HideInInspector] public float Zoom = 1;

        public bool ActiveStaticObject;
        private Vector3 lastPalayerPosition;
        public Transform playerPosition;
        private void Start()
        {
            AddMarkerObject();
        }
        private void Update()
        {
            if (playerPosition.position != lastPalayerPosition)
            {
                UpdateMarkersPosition();
                lastPalayerPosition = playerPosition.position;
            }
            UpdateMerkersPositionObject();
        }

        public void UpdateMarkersPosition()
        {
            RemoveNullCells();
            RemoveNullCellsStaticMarker();
            if (ActiveStaticObject)
            {
                foreach (var marker in StaticMarkers)
                {
                    // Поскольку мы уже удалили нулевые маркеры, мы можем предположить, что все маркеры действительны.
                    marker.SetPosition(CalculatePosition(marker));
                }
            }

            // Обновляем вращение опорного маркера.
            if (ReferenceMarker != null && ReferenceMarker.Object != null)
            {
                ReferenceMarker.transform.rotation = Quaternion.Euler(0f, 0f, -ReferenceMarker.Object.transform.rotation.eulerAngles.y);
            }
        }
        void UpdateMerkersPositionObject()
        {
            foreach (var marker in Markers)
            {
                // Поскольку мы уже удалили нулевые маркеры, мы можем предположить, что все маркеры действительны.
                marker.SetPosition(CalculatePosition(marker));
                marker.transform.localEulerAngles = new Vector3(0, 0, -marker.Object.transform.eulerAngles.y);
            }
        }

        public void AddMarkerObject(GameObject marker)
        {
            Objects.Add(marker.GetComponent<MapObject>());
        }
        public void AddMarkerObjectStatic(GameObject marker)
        {
            StaticObjects.Add(marker.GetComponent<MapObject>());
        }
        public void AddMarkerObject()
        {
            foreach (var obj in Objects)
                AddMarker(obj, false);
            AddMarker(Player, true);
        }
        public void AddMarkerStaticObject()
        {
            foreach (var obj in StaticObjects)
                AddMarkerStatic(obj, false);
        }

        private Vector2 CalculatePosition(MapMarker marker)
        {
            Vector2 mapSize = rectTransform.rect.size;
            Vector2 worldToMap = new Vector2(mapSize.x / WorldSize.x, mapSize.y / WorldSize.y);
            Vector2 difference = marker.Object.Position() - Player.Position();

            return new Vector2(difference.x * worldToMap.x * Zoom, difference.y * worldToMap.y * Zoom);
        }

        public void AddMarker(MapObject obj, bool reference)
        {
            MapMarker marker = Instantiate(MarkerPrefab, reference ? transform : markersTransform);
            marker.Initialize(obj, obj.gameObject, MarkerSize);

            obj.GetComponent<MapObject>().map = GetComponent<Map>();
            obj.GetComponent<MapObject>().mapMarker = marker.gameObject;

            if (!reference)
            {
                Markers.Add(marker);
            }
            else
                ReferenceMarker = marker;
        }
        public void AddMarkerStatic(MapObject obj, bool reference)
        {
            MapMarker marker = Instantiate(MarkerPrefab, reference ? transform : markersTransform);
            marker.Initialize(obj, obj.gameObject, MarkerSize);

            obj.GetComponent<MapObject>().map = GetComponent<Map>();
            obj.GetComponent<MapObject>().mapMarker = marker.gameObject;

            if (!reference)
            {
                StaticMarkers.Add(marker);
            }
            else
                ReferenceMarker = marker;
        }
        public void RemoveNullCellsStaticMarker()
        {
            StaticMarkers.RemoveAll(marker => marker == null || marker.Object == null);
            StaticObjects.RemoveAll(obj => obj == null || obj.gameObject == null);
        }
        public void RemoveNullCells()
        {
            Markers.RemoveAll(marker => marker == null || marker.Object == null);
            Objects.RemoveAll(obj => obj == null || obj.gameObject == null);
        }
    }
}