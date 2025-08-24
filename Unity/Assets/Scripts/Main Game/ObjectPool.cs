using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance { get; private set; }

        [Header("Object Pooling")]
        private List<GameObject> pooledObjects = new List<GameObject>();
        private int amountToPool = 50;
        [SerializeField] private GameObject bulletPrefab;

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
        }
        private void Start()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObjects()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            return null;
        }
    }
}
