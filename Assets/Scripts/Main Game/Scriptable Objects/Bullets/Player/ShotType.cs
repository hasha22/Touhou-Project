using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Shot Type")]
    public class ShotType : ScriptableObject
    {
        [Header("Bullet Data")]
        public GameObject bulletPrefab;
        public Sprite sprite;
        public Sprite empoweredSprite;
        public Sprite spriteAfterImage;
        public Sprite empoweredSpriteAfterImage;
        public float speed;
        public int damage;
        public AudioClip shootingSFX;
        public Vector2 spawnOffset1;
        public Vector2 spawnOffset2;
        public Vector2 spawnOffset3;
        public Vector2 spawnOffset4;
    }
}

