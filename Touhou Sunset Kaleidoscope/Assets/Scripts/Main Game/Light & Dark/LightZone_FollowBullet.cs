using UnityEngine;
namespace KH
{
    public class LightZone_FollowBullet : LightZone
    {
        private Transform targetBullet;
        public float radius = 1.5f;

        public void AttachToBullet(Transform bullet)
        {
            targetBullet = bullet;
        }
        protected override void InitializeLightZone(Vector2 position)
        {
            transform.position = position;
        }
        private void Update()
        {
            if (targetBullet != null)
            {
                transform.position = targetBullet.position;
            }
        }
    }
}

