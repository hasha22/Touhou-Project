using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Enemy/Boss Enemy")]
    public class Boss : ScriptableObject
    {
        [Header("Boss Information")]
        public string bossName;
        public Sprite bossSprite;
        public int bossID;
        public GameObject bossPrefab;
        public int hitScore;

        [Header("Phases")]
        public BossPhase[] phases;

        [Header("Collider Size")]
        public Vector2 colliderSize;
        public Vector2 colliderOffset;

        [Header("Spawn Settings")]
        public float spawnTimeInStage;
        public Vector3 spawnPoint;
    }
}

