using UnityEngine;
namespace KH
{
    public class BulletController : MonoBehaviour
    {
        [Header("Universal Bullet Behavior")]
        [HideInInspector] public float bulletSpeed;
        private Vector2 direction;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private PlayerMovement playerMovement;

        [Header("Player Bullet")]
        [HideInInspector] public int bulletDamage;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = PlayerInputManager.instance.playerObject.GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {

            if (gameObject.CompareTag("Player Bullet"))
            {
                rb.MovePosition(rb.position + direction * bulletSpeed * Time.fixedDeltaTime);
            }

        }
        public void InitializePlayerBullet(Vector2 dir, float speed, Sprite sprite, Sprite afterImageSprite, int damage, Vector2 playerVelocity)
        {
            Rigidbody2D playerRb = PlayerInputManager.instance.playerObject.GetComponent<Rigidbody2D>();
            direction = dir.normalized;
            bulletSpeed = speed;
            spriteRenderer.sprite = sprite;
            bulletDamage = damage;

            //rb.linearVelocity = (direction.normalized * speed) - new Vector2(0f, playerVelocity.y);

            GameObject afterImage = ObjectPool.instance.GetPooledPlayerBulletAfterImage();
            Afterimage img = afterImage.GetComponent<Afterimage>();
            img.InitializeAfterImage(Vector2.up, speed, afterImageSprite, transform.position, playerRb.linearVelocity);
        }
        public void InitializeEnemyBullet(Vector2 dir, float speed, Sprite sprite, BulletType bulletType)
        {
            rb.linearVelocity = dir.normalized * speed;
            spriteRenderer.sprite = sprite;

            // Removes collider, then reapplies the proper one
            Collider2D collider = GetComponent<Collider2D>();
            DestroyImmediate(collider);

            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;

            switch (bulletType.colliderType)
            {
                case ColliderType.Box:
                    BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();
                    boxCol.isTrigger = true;
                    boxCol.size = new Vector2(spriteWidth * bulletType.boxSize.x, spriteHeight * bulletType.boxSize.y);
                    boxCol.offset = new Vector2(spriteWidth * bulletType.boxOffset.x, spriteHeight * bulletType.boxOffset.y);
                    break;

                case ColliderType.Circle:
                    CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();
                    circleCol.isTrigger = true;
                    circleCol.radius = Mathf.Max(spriteWidth, spriteHeight) * bulletType.circleRadius;
                    circleCol.offset = new Vector2(spriteWidth * bulletType.circleOffset.x, spriteHeight * bulletType.circleOffset.y);
                    break;

                case ColliderType.Capsule:
                    CapsuleCollider2D capsuleCol = gameObject.AddComponent<CapsuleCollider2D>();
                    capsuleCol.isTrigger = true;
                    capsuleCol.size = new Vector2(spriteWidth * bulletType.capsuleSize.x, spriteHeight * bulletType.capsuleSize.y);
                    capsuleCol.offset = new Vector2(spriteWidth * bulletType.capsuleOffset.x, spriteHeight * bulletType.capsuleOffset.y);
                    capsuleCol.direction = bulletType.capsuleDirection;
                    break;
            }
        }

    }
}

