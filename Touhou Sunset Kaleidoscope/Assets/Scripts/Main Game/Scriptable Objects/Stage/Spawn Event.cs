using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Spawn Event")]
    public class SpawnEvent : ScriptableObject
    {
        public int enemyID;
        public Vector3 spawnPoint;
        public float spawnTime;
    }
}
