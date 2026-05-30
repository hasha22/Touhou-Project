using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Singular Bullet")]
    public class SingularBullet : EnemyShotPattern
    {
        public override void Fire(Vector2 origin, GameObject enemy)
        {
            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.InitializeEnemyBullet(new Vector2(0, -1), defaultBulletSpeed, bulletTypes[0].sprite, bulletTypes[0]);

            if (attackSounds[0] != null)
            {
                AudioManager.instance.PlaySFX(attackSounds[0], AudioManager.instance.enemyAudioSource.transform, attackSoundVolume);
            }
        }
    }
}

