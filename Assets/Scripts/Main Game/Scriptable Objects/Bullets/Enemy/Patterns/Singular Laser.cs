using System.Collections;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Singular Laser")]
    public class SingularLaser : EnemyShotPattern
    {
        [Header("Laser Data")]
        public Vector2 laserDirectionAngle;

        [Header("Laser Acceleration")]
        public AnimationCurve initialBulletAcceleration = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float accelerationDuration = 3f;
        public float delayBeforeAcceleration = 1f;
        public float accelerationSpeed = 3f;

        public override void Fire(Vector2 origin, GameObject enemy)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StartCoroutine(FireLaser(origin));
        }
        private IEnumerator FireLaser(Vector2 origin)
        {
            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);
            BulletController bulletController = bullet.GetComponent<BulletController>();

            // direction
            Vector3 meowDirection;
            if (laserDirectionAngle == Vector2.zero)
                meowDirection = PlayerInputManager.instance.playerObject.transform.position - bullet.transform.position;
            else
                meowDirection = laserDirectionAngle;

            Vector2 laserDirection = meowDirection.normalized;

            // bullet rotation 
            float rotationAngle = Mathf.Atan2(laserDirection.y, laserDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle - 90f);

            bulletController.InitializeEnemyBullet(laserDirection, defaultBulletSpeed, bulletTypes[0].sprite, bulletTypes[0]);
            bulletController.StopMovement(delayBeforeAcceleration);

            if (attackSounds[0] != null)
            {
                AudioManager.instance.PlaySFX(attackSounds[0], AudioManager.instance.enemyAudioSource.transform, attackSoundVolume);
            }

            yield return new WaitForSeconds(delayBeforeAcceleration);

            bulletController.StartAcceleration(initialBulletAcceleration, accelerationDuration);
            bulletController.InitializeEnemyBullet(laserDirection, accelerationSpeed, bulletTypes[0].sprite, bulletTypes[0]);

        }
    }
}

