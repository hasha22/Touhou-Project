using System.Collections;
using UnityEngine;
namespace KH

{

    [CreateAssetMenu(menuName = "Patterns/Boss Patterns/Icicle Rain")]


    public class IcicleRain : EnemyShotPattern
    {

        [Header("Icicle Data")]
        public float icicleDirectionAngle;

        [Header("Icicle Acceleration")]
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
            float angle = icicleDirectionAngle;
            float radians = angle * Mathf.Deg2Rad;
            Vector2 laserDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

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
