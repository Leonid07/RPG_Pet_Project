using UnityEngine;

namespace Radar
{
    public class MapObject : MonoBehaviour
    {
        public MapMarkerData Data;
        [HideInInspector] public GameObject mapMarker;
        [HideInInspector] public Map map;

        public Vector2 Position()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.z);
            return position;
        }
    }
}