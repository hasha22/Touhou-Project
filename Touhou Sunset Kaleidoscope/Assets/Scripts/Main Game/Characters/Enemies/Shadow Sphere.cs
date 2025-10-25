using UnityEngine;
namespace KH
{
    public class ShadowSphere : MonoBehaviour
    {
        [Header("Shadow Sphere Settings")]
        public float radius = 1.5f;
        public float duration = 4f;
        private float lifetime;

        private void OnEnable()
        {
            lifetime = duration;
        }
        void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
                gameObject.SetActive(false);
        }

        public bool Blocks(Vector2 position)
        {
            return Vector2.Distance(transform.position, position) <= radius;
        }
    }
}
