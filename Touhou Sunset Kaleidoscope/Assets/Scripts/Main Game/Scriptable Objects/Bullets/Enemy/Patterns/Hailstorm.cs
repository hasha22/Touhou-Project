using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Patterns/Boss Patterns/Hail Storm")]
    public class Hailstorm : EnemyShotPattern
    {
        [Header("Hailstorm Data")]
        public int numberOfBursts = 5;
        public float delayBetweenBursts = 0.5f;
        public float spreadDegrees = 360f;
        public float startAngle = 0f;
        public float minBurstOffset = 0.2f;
        public float maxBurstOffset = 0.6f;

        [Header("Step Data")]
        public int steps = 12;
        public int bulletsPerStep = 3;
        public int angleBetweenSteps = 30;
        public float bulletSpacing = 0.1f;

        [Header("Bullet Motion")]
        public AnimationCurve initialBulletDeceleration = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public float decelerationDuration = 1f;
        public float initialSpeed = 4f;

        [Header("Redirect Motion")]
        public float delayBeforeRedirect = 1f;
        public float redirectSpeed = 3f;
        public float redirectAngleOffset = 15f;

        [Header("Routines")]
        private Coroutine hailStormRoutine;
        private bool stopHail;

        public override void Fire(Vector2 origin)
        {
            BossManager boss = EnemyDatabase.instance.currentActiveBoss;
            if (hailStormRoutine == null)
            {
                hailStormRoutine = boss.StartCoroutine(HailStormRoutine(origin));
            }
        }
        private IEnumerator HailStormRoutine(Vector2 origin)
        {
            BossManager boss = EnemyDatabase.instance.currentActiveBoss;
            stopHail = false;

            // create a list to pass to the coroutine to get a reference to the bullets that must be redirected
            List<BulletController> bulletsToRedirect = new List<BulletController>();

            // loop for bursts
            for (int i = 0; i < numberOfBursts; i++)
            {
                if (stopHail) yield break;

                // small offset 
                float horizontalDir = (Random.value < 0.5f) ? -1f : 1f;
                float horizontalOffsetAmount = Random.Range(minBurstOffset, maxBurstOffset);
                Vector2 burstOffset = new Vector2(horizontalDir * horizontalOffsetAmount, 0f);
                Vector2 burstOrigin = origin + burstOffset;

                // fire 12 steps = 360
                for (int step = 0; step < steps; step++)
                {
                    if (stopHail) break;
                    //0, 30, 60, 90 etc.
                    float baseAngle = step * angleBetweenSteps;

                    //0, π/6, π/3, π/2 etc.
                    float radians = baseAngle * Mathf.Deg2Rad;

                    // (1, 0) , (0.86, 0.5), (0.5, 0.86), (0, 1) etc.
                    Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                    for (int j = 0; j < bulletsPerStep; j++)
                    {
                        // spawn position for bullets
                        Vector2 backwardOffset = -direction.normalized * ((j - 1) * bulletSpacing);
                        Vector2 spawnPos = burstOrigin + backwardOffset;

                        GameObject bullet = ObjectPool.instance.SpawnBullet(spawnPos);
                        BulletController bulletController = bullet.GetComponent<BulletController>();

                        bulletsToRedirect.Add(bulletController);

                        // bullet rotation 
                        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle - 90f);

                        bulletController.InitializeEnemyBullet(direction, initialSpeed, bulletType.sprite, bulletType);
                        bulletController.StartDeceleration(initialBulletDeceleration, decelerationDuration);
                    }
                }
                yield return new WaitForSeconds(delayBetweenBursts);

            }

            yield return new WaitForSeconds(delayBeforeRedirect);

            // redirect logic
            foreach (BulletController bc in bulletsToRedirect)
            {
                // 0, 15, 30, 45 etc.
                float baseAngle = bulletsToRedirect.IndexOf(bc) * redirectAngleOffset;

                // [-30, 30]
                float randomOffset = Random.Range(15f, 30f) * (Random.value < 0.5f ? -1f : 1f);

                // combines them
                float newAngle = baseAngle + randomOffset;

                //clamps to ensure bullets travels downward, between 180 and 360 degrees in world space
                if (newAngle < 180f) newAngle += 180f;
                if (newAngle > 360f) newAngle -= 180f;
                Vector2 newDir = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));

                //rotation again
                float rotationAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
                bc.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle - 90f);

                bc.InitializeEnemyBullet(newDir, redirectSpeed, bulletType.sprite, bulletType);
            }
            hailStormRoutine = null;
            stopHail = false;
        }
        public void StopPattern()
        {
            BossManager boss = EnemyDatabase.instance.currentActiveBoss;
            if (hailStormRoutine != null)
            {
                boss.StopCoroutine(hailStormRoutine);
                hailStormRoutine = null;
            }

            stopHail = true;
        }
        public void StartPattern(Vector2 origin)
        {
            BossManager boss = EnemyDatabase.instance.currentActiveBoss;
            if (hailStormRoutine == null)
            {
                hailStormRoutine = boss.StartCoroutine(HailStormRoutine(origin));
            }

            stopHail = false;
        }
    }
}
