using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Pillar Light Spawn Event")]
    public class PillarLightSpawnEvent : ScriptableObject
    {
        public LightZoneSize pillarZoneSize;
        public Vector2 direction;
        public Vector3 spawnPosition;
        public float spawnTime;
    }
}