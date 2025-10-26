using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Circular Light Spawn Event")]
    public class CircularLightSpawnEvent : ScriptableObject
    {
        public LightZoneSize circularZoneSize;
        public Vector3 spawnPosition;
        public float spawnTime;
    }
}

