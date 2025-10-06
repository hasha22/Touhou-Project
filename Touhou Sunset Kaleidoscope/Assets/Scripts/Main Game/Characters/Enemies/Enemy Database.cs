using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class EnemyDatabase : MonoBehaviour
    {
        public static EnemyDatabase instance { get; private set; }

        [Header("Enemies")]
        public List<Enemy> enemies = new List<Enemy>();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void SpawnEnemy(SpawnEvent data)
        {
            Enemy enemy = GetEnemyByID(data.enemyID);

            GameObject enemyPrefab = ObjectPool.instance.GetPooledEnemyObject();
            EnemyController enemyController = enemyPrefab.GetComponent<EnemyController>();
            if (enemy == null) Debug.Log("Meow");
            enemyController.InitializeEnemy(enemy, data.spawnPosition);
            enemyController.InitializeAttackSequence(enemy.attackSequence);
        }
        public Enemy GetEnemyByID(int id)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.enemyID == id)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
}

