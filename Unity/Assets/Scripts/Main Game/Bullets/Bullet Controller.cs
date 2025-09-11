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

        [Header("Player Bullet")]
        [HideInInspector] public int bulletDamage;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (gameObject.CompareTag("Player Bullet"))
            {
                rb.MovePosition(rb.position + direction * bulletSpeed * Time.fixedDeltaTime);
            }
        }

        public void InitializePlayerBullet(Vector2 dir, float speed, Sprite sprite, int damage)
        {
            direction = dir.normalized;
            bulletSpeed = speed;
            spriteRenderer.sprite = sprite;
            bulletDamage = damage;
        }
        public void InitializeEnemyBullet(Vector2 dir, float speed, Sprite sprite)
        {
            rb.linearVelocity = dir.normalized * speed;
            spriteRenderer.sprite = sprite;
        }

    }
}

