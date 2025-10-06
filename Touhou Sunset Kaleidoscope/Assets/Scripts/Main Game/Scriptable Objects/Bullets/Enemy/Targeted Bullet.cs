using KH;
using UnityEngine;
[CreateAssetMenu(menuName = "Patterns/Targeted Pattern")]
public class TargetedBullet : EnemyShotPattern
{
    public override void Fire(Vector2 origin)
    {
        Vector2 playerPosition = PlayerInputManager.instance.playerObject.transform.position;
        Vector2 dir = (playerPosition - origin).normalized;

        GameObject bullet = ObjectPool.instance.SpawnBullet(origin);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.InitializeEnemyBullet(dir, defaultBulletSpeed, bulletSprite);
    }
}
