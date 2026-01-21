using UnityEngine;
namespace KH
{
    public class LightZone_FollowBullet : LightZoneBase
    {
        private Transform target;
        public float radius = 1.5f;

        public void AttachToBullet(Transform bullet)
        {
            target = bullet;
        }
        public override void Initialize(Vector2 position, LightZoneSize zoneSize)
        {
            transform.position = position;
        }
        private void LateUpdate()
        {
            if (target != null)
                transform.position = target.position;
        }
    }
}

