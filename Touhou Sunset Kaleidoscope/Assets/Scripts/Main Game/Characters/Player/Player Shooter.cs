using UnityEngine;
namespace KH
{
    public class PlayerShooter : MonoBehaviour
    {
        PlayerManager playerManager;

        [Header("Shooter Setup")]
        [SerializeField] private float fireRate = 0.1f;
        private float fireTimer = 0f;
        private bool shootingInput = false;
        [SerializeField] private float damageMultiplier = 0.2f;

        // It is crucial to separate input, which needs to be detected every frame, from shooting
        // which needs to be frame rate independent. FixedUpdate is called every 0.02s (50 frames / second), thus
        // making the player shooting constant, with equal gaps between volleys, regardless
        // of frame rate. FixedUpdate is also ideal for applying forces to RigidBodies
        // and checking for collisions.

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }
        void Update()
        {
            shootingInput = PlayerInputManager.instance.isShooting;
        }
        private void FixedUpdate()
        {
            if (shootingInput)
            {
                // Time since last frame
                fireTimer += Time.fixedDeltaTime;

                // Catches up if frame took too long
                while (fireTimer >= fireRate)
                {
                    Shoot();
                    fireTimer -= fireRate;
                }
            }
            else
            {
                fireTimer = 0f; // Reset if the player stops shooting
            }
        }
        private void Shoot()
        {
            // This method handles shooting for the player

            // Player bullets spawn position
            Vector3 spawnPosition1 = transform.position + (Vector3)playerManager.characterData.shotType.spawnOffset1;
            Vector3 spawnPosition2 = transform.position + (Vector3)playerManager.characterData.shotType.spawnOffset2;

            // Grabs bullets from pool
            GameObject bulletObject1 = ObjectPool.instance.GetPooledPlayerObject();
            GameObject bulletObject2 = ObjectPool.instance.GetPooledPlayerObject();

            if (bulletObject1 != null && bulletObject2 != null)
            {
                bulletObject1.transform.position = spawnPosition1;
                bulletObject2.transform.position = spawnPosition2;

                float damage = playerManager.characterData.shotType.damage * (1 + playerManager.currentPower * damageMultiplier);
                int intDamage = Mathf.RoundToInt(damage);

                // Initializing bullet data
                BulletController bullet1 = bulletObject1.GetComponent<BulletController>();
                bullet1.InitializePlayerBullet(Vector2.up, playerManager.characterData.shotType.speed, playerManager.characterData.shotType.sprite, intDamage);

                BulletController bullet2 = bulletObject2.GetComponent<BulletController>();
                bullet2.InitializePlayerBullet(Vector2.up, playerManager.characterData.shotType.speed, playerManager.characterData.shotType.sprite, intDamage);

            }
        }
    }
}