using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance { get; private set; }

        [Header("Object Pooling")]
        private List<GameObject> pooledObjects = new List<GameObject>();
        [SerializeField] private int amountToPool = 200;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletContainer;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(bulletContainer);

                bullet.SetActive(false);
                pooledObjects.Add(bullet);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    pooledObjects[i].SetActive(true);
                    return pooledObjects[i];
                }
            }
            return null;
        }
        public void ReturnToPool(GameObject bullet)
        {
            bullet.SetActive(false);
        }
    }
}
