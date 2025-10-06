using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance { get; private set; }

        [Header("Object Pooling")]
        private List<GameObject> pooledPlayerBullets = new List<GameObject>();
        private List<GameObject> pooledEnemyBullets = new List<GameObject>();
        private List<GameObject> pooledItems = new List<GameObject>();
        private List<GameObject> pooledEnemyObjects = new List<GameObject>();
        [SerializeField] private int amountToPool1 = 100;
        [SerializeField] private int amountToPool2 = 100;
        [SerializeField] private int amountToPool3 = 100;
        [SerializeField] private int amountToPool4 = 100;
        [Space]
        [SerializeField] private GameObject playerBulletPrefab;
        [SerializeField] private GameObject enemyBulletPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject enemyObjectPrefab;
        [Space]
        [SerializeField] private Transform playerBulletsContainer;
        [SerializeField] private Transform enemyBulletsContainer;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private Transform enemyContainer;

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
            for (int i = 0; i < amountToPool1; i++)
            {
                GameObject bullet = Instantiate(playerBulletPrefab);
                bullet.transform.SetParent(playerBulletsContainer);

                bullet.SetActive(false);
                pooledPlayerBullets.Add(bullet);
            }
            for (int i = 0; i < amountToPool2; i++)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab);
                bullet.transform.SetParent(enemyBulletsContainer);

                bullet.SetActive(false);
                pooledEnemyBullets.Add(bullet);
            }
            for (int i = 0; i < amountToPool3; i++)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(itemContainer);

                item.SetActive(false);
                pooledItems.Add(item);
            }
            for (int i = 0; i < amountToPool4; i++)
            {
                GameObject enemy = Instantiate(enemyObjectPrefab);
                enemy.transform.SetParent(enemyContainer);
                enemy.SetActive(false);
                pooledEnemyObjects.Add(enemy);
            }
        }

        public GameObject GetPooledPlayerObject()
        {
            for (int i = 0; i < pooledPlayerBullets.Count; i++)
            {
                if (!pooledPlayerBullets[i].activeInHierarchy)
                {
                    pooledPlayerBullets[i].SetActive(true);
                    return pooledPlayerBullets[i];
                }
            }
            return null;
        }
        public GameObject GetPooledEnemyBullet()
        {
            for (int i = 0; i < pooledEnemyBullets.Count; i++)
            {
                if (!pooledEnemyBullets[i].activeInHierarchy)
                {
                    pooledEnemyBullets[i].SetActive(true);
                    return pooledEnemyBullets[i];
                }
            }
            return null;
        }
        public GameObject GetPooledItem()
        {
            for (int i = 0; i < pooledItems.Count; i++)
            {
                if (!pooledItems[i].activeInHierarchy)
                {
                    pooledItems[i].SetActive(true);
                    return pooledItems[i];
                }
            }
            return null;
        }
        public GameObject GetPooledEnemyObject()
        {
            for (int i = 0; i < pooledEnemyObjects.Count; i++)
            {
                if (!pooledEnemyObjects[i].activeInHierarchy)
                {
                    pooledEnemyObjects[i].SetActive(true);
                    return pooledEnemyObjects[i];
                }
            }
            return null;
        }
        public void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        public List<GameObject> GetItemPool()
        {
            return pooledItems;
        }
        public GameObject SpawnBullet(Vector2 worldPos)
        {
            GameObject bullet = GetPooledEnemyBullet();
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.position = worldPos;
            rb.linearVelocity = Vector2.zero;

            bullet.SetActive(true);
            return bullet;
        }
    }
}
