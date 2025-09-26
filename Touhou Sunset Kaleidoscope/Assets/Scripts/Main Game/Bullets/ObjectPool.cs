using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance { get; private set; }

        [Header("Object Pooling")]
        private List<GameObject> pooledPlayerObjects = new List<GameObject>();
        private List<GameObject> pooledEnemyObjects = new List<GameObject>();
        private List<GameObject> pooledItems = new List<GameObject>();
        [SerializeField] private int amountToPool1 = 100;
        [SerializeField] private int amountToPool2 = 100;
        [SerializeField] private int amountToPool3 = 100;
        [SerializeField] private GameObject playerBulletPrefab;
        [SerializeField] private GameObject enemyBulletPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform playerBulletsContainer;
        [SerializeField] private Transform enemyBulletsContainer;
        [SerializeField] private Transform itemContainer;

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
                pooledPlayerObjects.Add(bullet);
            }
            for (int i = 0; i < amountToPool2; i++)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab);
                bullet.transform.SetParent(enemyBulletsContainer);

                bullet.SetActive(false);
                pooledEnemyObjects.Add(bullet);
            }
            for (int i = 0; i < amountToPool3; i++)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(itemContainer);

                item.SetActive(false);
                pooledItems.Add(item);
            }
        }

        public GameObject GetPooledPlayerObject()
        {
            for (int i = 0; i < pooledPlayerObjects.Count; i++)
            {
                if (!pooledPlayerObjects[i].activeInHierarchy)
                {
                    pooledPlayerObjects[i].SetActive(true);
                    return pooledPlayerObjects[i];
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
        public void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        public GameObject SpawnBullet(Vector2 worldPos)
        {
            GameObject bullet = GetPooledEnemyObject();
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.position = worldPos;
            rb.linearVelocity = Vector2.zero;

            bullet.SetActive(true);
            return bullet;
        }
    }
}
