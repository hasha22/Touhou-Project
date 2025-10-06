using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Spawn Event")]
    public class SpawnEvent : ScriptableObject
    {
        public int enemyID;
        public Vector2 spawnPosition;
        public float spawnTime;
    }
}
