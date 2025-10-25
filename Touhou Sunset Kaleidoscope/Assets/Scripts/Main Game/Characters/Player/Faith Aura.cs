using UnityEngine;
namespace KH
{
    public class FaithAura : MonoBehaviour, ILuminousZone
    {
        [Header("Aura Settings")]
        public float radius = 2f;
        public Color auraColor = Color.red;
        public float slowFactor = 0.7f;

        private Transform player;

        public float Intensity => 1f;
        public Color ZoneColor => auraColor;

        public void AttachToPlayer(FaithManager manager)
        {
            player = manager.transform;
        }
        void LateUpdate()
        {
            if (player != null)
                transform.position = player.position;
        }
        public bool ContainsPoint(Vector2 point)
        {
            return Vector2.Distance(transform.position, point) <= radius;
        }
    }

}

