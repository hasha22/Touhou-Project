using UnityEngine;
namespace KH
{
    public abstract class LightZoneBase : MonoBehaviour, ILuminousZone
    {
        [Header("Light Settings")]
        public float intensity = 1f;
        public Color zoneColor = Color.white;

        protected float lifeTime;
        protected Collider2D zoneCollider;

        public float Intensity => intensity;
        public Color ZoneColor => zoneColor;

        protected virtual void Awake()
        {
            //zoneCollider = GetComponent<Collider2D>();
            //zoneCollider.isTrigger = true;
        }
        protected virtual void OnEnable()
        {
        }
        protected virtual void Update()
        {

        }
        public bool ContainsPoint(Vector2 point)
        {
            return zoneCollider != null && zoneCollider.OverlapPoint(point);
        }
        public abstract void Initialize(Vector2 position, LightZoneSize zoneSize);
    }
}


