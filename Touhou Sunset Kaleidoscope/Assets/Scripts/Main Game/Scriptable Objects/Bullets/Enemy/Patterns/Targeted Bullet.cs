using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Targeted Pattern")]
    public class TargetedBullet : EnemyShotPattern
    {
        public override void Fire(Vector2 origin)
        {
            // calculates direction towards player
            Vector2 playerPosition = PlayerInputManager.instance.playerObject.transform.position;
            Vector2 directionToPlayer = (playerPosition - origin).normalized;

            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);

            // rotates bullet towards player
            // Atan2 converts direction vector to an angle in radians. for example x, y = 0.5 => Atan returns ? / 4, cause arctan(0.5 / 0.5) = ? / 4
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // subtracting by 90f because sprite is facing up.

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.InitializeEnemyBullet(directionToPlayer, defaultBulletSpeed, bulletType.sprite, bulletType);
        }
    }
}
