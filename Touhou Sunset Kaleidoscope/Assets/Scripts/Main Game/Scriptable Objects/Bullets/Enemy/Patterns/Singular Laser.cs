using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
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

        [Header("Routines")]
        private Coroutine laserRoutine;
        public override void Fire(Vector2 origin, GameObject enemy)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (laserRoutine == null)
            {
                laserRoutine = enemyController.StartCoroutine(FireLaser(origin));
            }
        }
        private IEnumerator FireLaser(Vector2 origin)
        {
            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);
            BulletController bulletController = bullet.GetComponent<BulletController>();

            // direction
            Vector3 meowDirection;
            if (laserDirectionAngle == Vector2.zero)
            {
                meowDirection = PlayerInputManager.instance.playerObject.transform.position - bullet.transform.position;
            }
            else
            {
                meowDirection = laserDirectionAngle;
            }
            Vector2 laserDirection = meowDirection.normalized;

            // bullet rotation 
            float rotationAngle = Mathf.Atan2(laserDirection.y, laserDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle - 90f);

            bulletController.InitializeEnemyBullet(laserDirection, defaultBulletSpeed, bulletType.sprite, bulletType);
            bulletController.StopMovement(delayBeforeAcceleration);

            yield return new WaitForSeconds(delayBeforeAcceleration);

            bulletController.StartAcceleration(initialBulletAcceleration, accelerationDuration);
            bulletController.InitializeEnemyBullet(laserDirection, accelerationSpeed, bulletType.sprite, bulletType);

            laserRoutine = null;
        }
    }
}

