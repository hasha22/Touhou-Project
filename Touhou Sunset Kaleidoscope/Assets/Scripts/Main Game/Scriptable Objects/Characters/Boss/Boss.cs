using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Enemy/Boss Enemy")]
    public class Boss : ScriptableObject
    {
        [Header("Boss Information")]
        public string bossName;
        public Sprite bossSprite;
        public int bossHealth;
        public int bossID;
        public GameObject bossPrefab;

        [Header("Collider Size")]
        public Vector2 colliderSize;
        public Vector2 colliderOffset;
    }
}

