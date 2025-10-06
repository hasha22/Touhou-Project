using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Spawn Event")]
    public class SpawnEvent : ScriptableObject
    {
        public int enemyID;
        public Vector2 spawnPosition; // Testing needs to be done to determine the boundaries where enemies can spawn offscreen without hitting disabler wall.
        public float spawnTime;
    }
}
