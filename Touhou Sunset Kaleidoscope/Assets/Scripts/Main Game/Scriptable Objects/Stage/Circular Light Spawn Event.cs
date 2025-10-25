using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Light Spawn Event")]
    public class CircularLightSpawnEvent : ScriptableObject
    {
        public LightZoneSize circularZoneType;
        public Vector3 spawnPosition;
        public float spawnTime;
    }
}

