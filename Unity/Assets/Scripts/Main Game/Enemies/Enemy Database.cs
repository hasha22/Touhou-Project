using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class EnemyDatabase : MonoBehaviour
    {
        public static EnemyDatabase instance { get; private set; }

        [Header("Enemies")]
        public List<GameObject> enemies = new List<GameObject>();
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
    }
}

