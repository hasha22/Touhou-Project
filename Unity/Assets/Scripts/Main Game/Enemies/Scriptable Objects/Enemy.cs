using UnityEngine;
namespace KH
{
    public class Enemy : ScriptableObject
    {
        [Header("Enemy Information")]
        public string enemyName;
        public Sprite enemySprite;
        public int enemyHealth;
        public int enemyID;
        public GameObject enemyPrefab;
        public EnemyType enemyType;
    }
}
