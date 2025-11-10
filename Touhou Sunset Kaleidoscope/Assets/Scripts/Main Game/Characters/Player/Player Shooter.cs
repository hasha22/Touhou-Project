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
        public int currentBulletDamage = 0;

        [Header("Light Bullets")]
        [SerializeField] private Vector2 lightBulletDirection1;
        [SerializeField] private Vector2 lightBulletDirection2;

        [Header("Flags")]
        public bool isPaused = false;

        private Rigidbody2D rb;

        // It is crucial to separate input, which needs to be detected every frame, from shooting
        // which needs to be frame rate independent. FixedUpdate is called every 0.02s (50 frames / second), thus
        // making the player shooting constant, with equal gaps between volleys, regardless
        // of frame rate. FixedUpdate is also ideal for applying forces to RigidBodies
        // and checking for collisions.

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            rb = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            if (isPaused)
            {
                shootingInput = false;
                return;
            }
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
            GameObject bulletObject1 = ObjectPool.instance.GetPooledPlayerBullet();
            GameObject bulletObject2 = ObjectPool.instance.GetPooledPlayerBullet();

            if (bulletObject1 != null && bulletObject2 != null)
            {
                bulletObject1.transform.position = spawnPosition1;
                bulletObject2.transform.position = spawnPosition2;

                bulletObject1.SetActive(true);
                bulletObject2.SetActive(true);

                currentBulletDamage = playerManager.characterData.shotType.damage;
                float damage = currentBulletDamage * (1 + playerManager.currentPower * damageMultiplier) * playerManager.damageMultiplier;
                int intDamage = Mathf.RoundToInt(damage);

                Sprite mainBulletSprite = !LightZoneManager.instance.IsInLight(transform.position) ? playerManager.characterData.shotType.sprite : playerManager.characterData.shotType.empoweredSprite;
                Sprite mainBulletAfterImage = !LightZoneManager.instance.IsInLight(transform.position) ? playerManager.characterData.shotType.spriteAfterImage : playerManager.characterData.shotType.empoweredSpriteAfterImage;

                // Initializing bullet data
                BulletController bullet1 = bulletObject1.GetComponent<BulletController>();
                bullet1.InitializePlayerBullet(Vector2.up,
                    playerManager.characterData.shotType.speed,
                    mainBulletSprite,
                    mainBulletAfterImage,
                    intDamage,
                    rb.linearVelocity);

                BulletController bullet2 = bulletObject2.GetComponent<BulletController>();
                bullet2.InitializePlayerBullet(Vector2.up,
                    playerManager.characterData.shotType.speed,
                    mainBulletSprite,
                    mainBulletAfterImage,
                    intDamage,
                    rb.linearVelocity);

            }
            if (LightZoneManager.instance.IsInLight(transform.position))
            {
                Vector3 spawnPosition3 = transform.position + (Vector3)playerManager.characterData.shotType.spawnOffset3;
                Vector3 spawnPosition4 = transform.position + (Vector3)playerManager.characterData.shotType.spawnOffset4;

                GameObject bulletObject3 = ObjectPool.instance.GetPooledPlayerBullet();
                GameObject bulletObject4 = ObjectPool.instance.GetPooledPlayerBullet();

                currentBulletDamage = playerManager.characterData.shotType.damage;
                float damage = currentBulletDamage * (1 + playerManager.currentPower * damageMultiplier) * playerManager.damageMultiplier;
                int intDamage = Mathf.RoundToInt(damage);

                if (bulletObject3 != null && bulletObject4 != null)
                {
                    bulletObject3.transform.position = spawnPosition3;
                    bulletObject4.transform.position = spawnPosition4;

                    bulletObject3.SetActive(true);
                    bulletObject4.SetActive(true);

                    BulletController bullet3 = bulletObject3.GetComponent<BulletController>();
                    bullet3.InitializePlayerBullet(lightBulletDirection1,
                        playerManager.characterData.shotType.speed,
                        playerManager.characterData.shotType.sprite,
                        playerManager.characterData.shotType.spriteAfterImage,
                        intDamage,
                        rb.linearVelocity);

                    BulletController bullet4 = bulletObject4.GetComponent<BulletController>();
                    bullet4.InitializePlayerBullet(lightBulletDirection2,
                        playerManager.characterData.shotType.speed,
                        playerManager.characterData.shotType.sprite,
                        playerManager.characterData.shotType.spriteAfterImage,
                        intDamage,
                        rb.linearVelocity);
                }
            }
        }
    }
}