using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance { get; private set; }

        [Header("Object Pooling")]
        private List<GameObject> pooledPlayerBullets = new List<GameObject>();
        private List<GameObject> pooledPlayerBulletAfterImages = new List<GameObject>();
        private List<GameObject> pooledEnemyBullets = new List<GameObject>();
        private List<GameObject> pooledItems = new List<GameObject>();
        private List<GameObject> pooledEnemyObjects = new List<GameObject>();
        private List<GameObject> pooledPillarZones = new List<GameObject>();
        private List<GameObject> pooledCircularZones = new List<GameObject>();
        private List<GameObject> pooledFollowZones_Capsule = new List<GameObject>();
        private List<GameObject> pooledFollowZones_Circle = new List<GameObject>();
        private List<GameObject> pooledShadowZones = new List<GameObject>();
        [SerializeField] private int playerBulletsToPool = 100;
        [SerializeField] private int playerBulletAfterImagesToPool = 100;
        [SerializeField] private int enemyBulletsToPool = 500;
        [SerializeField] private int itemsToPool = 300;
        [SerializeField] private int enemiesToPool = 100;
        [SerializeField] private int pillarZonesToPool = 50;
        [SerializeField] private int circularZonesToPool = 50;
        [SerializeField] private int followZonesToPool_Capsule = 50;
        [SerializeField] private int followZonesToPool_Circle = 500;
        [SerializeField] private int shadowZonesToPool = 50;
        [Space]
        [SerializeField] private GameObject playerBulletPrefab;
        [SerializeField] private GameObject playerBulletAfterImagePrefab;
        [SerializeField] private GameObject enemyBulletPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject enemyObjectPrefab;
        [SerializeField] private GameObject pillarZonePrefab;
        [SerializeField] private GameObject circularZonePrefab;
        [SerializeField] private GameObject followZonePrefab_Capsule;
        [SerializeField] private GameObject followZonePrefab_Circle;
        [SerializeField] private GameObject shadowZonePrefab;
        [Space]
        [SerializeField] private Transform playerBulletsContainer;
        [SerializeField] private Transform playerBulletAfterImagesContainer;
        [SerializeField] private Transform enemyBulletsContainer;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private Transform lightZoneContainer;
        [SerializeField] private Transform followZoneContainer;
        [SerializeField] private Transform shadowZoneContainer;
        public Transform importantObjectsParent;

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
            for (int i = 0; i < playerBulletsToPool; i++)
            {
                GameObject bullet = Instantiate(playerBulletPrefab);
                bullet.transform.SetParent(playerBulletsContainer);

                bullet.SetActive(false);
                pooledPlayerBullets.Add(bullet);
            }
            for (int i = 0; i < enemyBulletsToPool; i++)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab);
                bullet.transform.SetParent(enemyBulletsContainer);

                bullet.SetActive(false);
                pooledEnemyBullets.Add(bullet);
            }
            for (int i = 0; i < itemsToPool; i++)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(itemContainer);

                item.SetActive(false);
                pooledItems.Add(item);
            }
            for (int i = 0; i < enemiesToPool; i++)
            {
                GameObject enemy = Instantiate(enemyObjectPrefab);
                enemy.transform.SetParent(enemyContainer);

                enemy.SetActive(false);
                pooledEnemyObjects.Add(enemy);
            }
            for (int i = 0; i < playerBulletAfterImagesToPool; i++)
            {
                GameObject afterImage = Instantiate(playerBulletAfterImagePrefab);
                afterImage.transform.SetParent(playerBulletAfterImagesContainer);

                afterImage.SetActive(false);
                pooledPlayerBulletAfterImages.Add(afterImage);
            }
            for (int i = 0; i < pillarZonesToPool; i++)
            {
                GameObject lightZone = Instantiate(pillarZonePrefab);
                lightZone.transform.SetParent(lightZoneContainer);

                lightZone.SetActive(false);
                pooledPillarZones.Add(lightZone);
            }
            for (int i = 0; i < circularZonesToPool; i++)
            {
                GameObject lightZone = Instantiate(circularZonePrefab);
                lightZone.transform.SetParent(lightZoneContainer);

                lightZone.SetActive(false);
                pooledCircularZones.Add(lightZone);
            }
            for (int i = 0; i < followZonesToPool_Capsule; i++)
            {
                GameObject lightZone = Instantiate(followZonePrefab_Capsule);
                lightZone.transform.SetParent(followZoneContainer);

                lightZone.SetActive(false);
                pooledFollowZones_Capsule.Add(lightZone);
            }
            for (int i = 0; i < followZonesToPool_Circle; i++)
            {
                GameObject lightZone = Instantiate(followZonePrefab_Circle);
                lightZone.transform.SetParent(followZoneContainer);

                lightZone.SetActive(false);
                pooledFollowZones_Circle.Add(lightZone);
            }
            for (int i = 0; i < shadowZonesToPool; i++)
            {
                GameObject shadowZone = Instantiate(shadowZonePrefab);
                shadowZone.transform.SetParent(shadowZoneContainer);

                shadowZone.SetActive(false);
                pooledShadowZones.Add(shadowZone);
            }
        }

        public GameObject GetPooledPlayerBullet()
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
                    return pooledEnemyObjects[i];
                }
            }
            return null;
        }
        public GameObject GetPooledPlayerBulletAfterImage()
        {
            for (int i = 0; i < pooledPlayerBulletAfterImages.Count; i++)
            {
                if (!pooledPlayerBulletAfterImages[i].activeInHierarchy)
                {
                    pooledPlayerBulletAfterImages[i].SetActive(true);
                    return pooledPlayerBulletAfterImages[i];
                }
            }
            return null;
        }
        public GameObject GetPooledPillarZone()
        {
            for (int i = 0; i < pooledPillarZones.Count; i++)
            {
                if (!pooledPillarZones[i].activeInHierarchy)
                {
                    return pooledPillarZones[i];
                }
            }
            return null;
        }
        public GameObject GetPooledCircularZone()
        {
            for (int i = 0; i < pooledCircularZones.Count; i++)
            {
                if (!pooledCircularZones[i].activeInHierarchy)
                {
                    return pooledCircularZones[i];
                }
            }
            return null;
        }
        public GameObject GetPooledFollowZone_Capsule()
        {
            for (int i = 0; i < pooledFollowZones_Capsule.Count; i++)
            {
                if (!pooledFollowZones_Capsule[i].activeInHierarchy)
                {
                    return pooledFollowZones_Capsule[i];
                }
            }
            return null;
        }
        public GameObject GetPooledFollowZone_Circle()
        {
            for (int i = 0; i < pooledFollowZones_Circle.Count; i++)
            {
                if (!pooledFollowZones_Circle[i].activeInHierarchy)
                {

                    return pooledFollowZones_Circle[i];
                }
            }
            return null;
        }
        public GameObject GetPooledShadowZone()
        {
            for (int i = 0; i < pooledShadowZones.Count; i++)
            {
                if (!pooledShadowZones[i].activeInHierarchy)
                {
                    return pooledShadowZones[i];
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

            Debug.Log(worldPos);

            bullet.transform.position = worldPos;
            rb.linearVelocity = Vector2.zero;

            bullet.SetActive(true);
            return bullet;
        }
        public List<GameObject> GetPooledEnemyBullets()
        {
            return pooledEnemyBullets;
        }
    }
}
