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
        public void InitializeEnemyBullet(Vector2 dir, float speed, Sprite sprite)
        {
            rb.linearVelocity = dir.normalized * speed;
            spriteRenderer.sprite = sprite;
        }

    }
}

