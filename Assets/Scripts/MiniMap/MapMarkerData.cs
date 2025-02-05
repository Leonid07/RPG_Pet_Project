using UnityEngine;

namespace Radar
{
    [CreateAssetMenu(menuName = "Map Marker", fileName = "New Map Marker")]
    public class MapMarkerData : ScriptableObject
    {
        [Header("Data")]
        public string Name;
        public Sprite Icon;
    }
}