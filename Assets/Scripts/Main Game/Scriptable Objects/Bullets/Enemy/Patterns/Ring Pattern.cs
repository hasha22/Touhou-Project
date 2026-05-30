using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Ring Pattern")]
    public class RingPattern : EnemyShotPattern
    {
        [Header("Ring Pattern Data")]
        public int count = 36;
        public float spreadDegrees = 360f;
        public float startAngle = 0f;

        // Each bullet pattern will contain it's own trigonometrical calculations to fire its specific pattern
        public override void Fire(Vector2 origin, GameObject enemy)
        {
            float angle = startAngle;
            float step = spreadDegrees / count;
            for (int i = 0; i < count; i++)
            {
                float radians = angle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                GameObject bullet = ObjectPool.instance.SpawnBullet(origin);

                BulletController bulletController = bullet.GetComponent<BulletController>();
                bulletController.InitializeEnemyBullet(direction, defaultBulletSpeed, bulletTypes[0].sprite, bulletTypes[0]);

                if (attackSounds[0] != null)
                {
                    AudioManager.instance.PlaySFX(attackSounds[0], AudioManager.instance.enemyAudioSource.transform, attackSoundVolume);
                }

                angle += step;
            }
        }
    }
}
