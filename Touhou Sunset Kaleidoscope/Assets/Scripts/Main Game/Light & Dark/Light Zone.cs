using UnityEngine;
namespace KH
{
    public abstract class LightZone : MonoBehaviour
    {
        [Header("Light Properties")]
        public float intensity = 1f;
        public Color lightColor = Color.white;
        public float duration = 3f;
        public Collider2D zoneCollider;
        protected float lifetime;

        protected virtual void Awake()
        {
            zoneCollider = GetComponent<Collider2D>();
            zoneCollider.isTrigger = true;
        }
        protected void OnEnable()
        {
            lifetime = duration;
        }
        protected virtual void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
                gameObject.SetActive(false); // pool later
        }
        protected virtual void InitializeLightZone(Vector2 position)
        {

        }


    }
}

