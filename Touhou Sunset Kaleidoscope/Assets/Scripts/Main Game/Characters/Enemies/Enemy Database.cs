using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class EnemyDatabase : MonoBehaviour
    {
        public static EnemyDatabase instance { get; private set; }

        [Header("Enemies")]
        public List<Enemy> enemies = new List<Enemy>();
        public List<Boss> bosses = new List<Boss>();
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
            enemyController.InitializeEnemy(enemy, data.spawnPoint);
            enemyController.InitializeAttackSequence(enemy.attackSequence);

            enemyPrefab.gameObject.SetActive(true);
        }
        public void SpawnBoss(SpawnEvent data)
        {

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
        public Boss GetBossByID(int id)
        {
            foreach (Boss boss in bosses)
            {
                if (boss.bossID == id)
                {
                    return boss;
                }
            }
            return null;
        }
    }
}

