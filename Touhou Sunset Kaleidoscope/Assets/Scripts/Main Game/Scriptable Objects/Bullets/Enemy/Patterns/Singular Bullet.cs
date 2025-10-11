using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Singular Bullet")]
    public class SingularBullet : EnemyShotPattern
    {
        public override void Fire(Vector2 origin)
        {
            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.InitializeEnemyBullet(new Vector2(0, -1), defaultBulletSpeed, bulletType.sprite, bulletType);
        }
    }
}

