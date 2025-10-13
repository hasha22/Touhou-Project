using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Enemy/Regular Enemy")]
    public class Enemy : ScriptableObject
    {
        [Header("Enemy Information")]
        public string enemyName;
        public int enemyHealth;
        public int enemyID;
        public int healthResetValue;
        public Sprite enemySprite;
        public GameObject enemyPrefab;
        public EnemyType enemyType;
        public ItemToSpawn itemToSpawn;
        public int numberOfItemsToSpawn;

        [Header("Score")]
        public int deathScore;
        public int hitScore;

        [Header("Collider Size")]
        public Vector2 colliderSize;
        public Vector2 colliderOffset;

        [Header("Enemy Shooting")]
        public AttackSequence attackSequence;
        public float delayBeforeAttack;

        [Header("Enemy Movement")]
        public MovementSequence movementSequence;
    }
}
