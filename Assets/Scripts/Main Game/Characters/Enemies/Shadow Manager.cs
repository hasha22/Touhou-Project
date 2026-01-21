using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ShadowManager : MonoBehaviour
    {
        public static ShadowManager instance { get; private set; }

        [Header("Shadows")]
        public List<ShadowSphere> activeShadows;
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
        public ShadowSphere SpawnShadow(Vector2 position)
        {

            GameObject sphere = ObjectPool.instance.GetPooledShadowZone();
            activeShadows.Add(sphere.GetComponent<ShadowSphere>());

            sphere.SetActive(true);
            return sphere.GetComponent<ShadowSphere>();

        }
        public bool IsBlockedByShadow(Vector2 point)
        {
            foreach (var s in activeShadows)
                if (s.Blocks(point))
                    return true;
            return false;
        }
    }
}
