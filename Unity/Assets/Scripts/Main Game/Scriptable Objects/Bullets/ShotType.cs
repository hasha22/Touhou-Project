using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Shot Pattern")]
    public class ShotType : ScriptableObject
    {
        [Header("Bullet Data")]
        public GameObject bulletPrefab;
        public Sprite sprite;
        public float speed;
        public float damage;
        public Vector2 spawnOffset1;
        public Vector2 spawnOffset2;
    }
}

