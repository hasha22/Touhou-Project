using UnityEngine;
using UnityEngine.UI;
namespace KH
{
    public class BulletController : MonoBehaviour
    {
        [Header("Universal Bullet Behavior")]
        [HideInInspector] public float bulletSpeed;
        private Vector2 direction;
        private Image bulletImage;
        private Rigidbody2D rb;

        [Header("Player Bullet")]
        [HideInInspector] public float bulletDamage = 0f;
        private void Awake()
        {
            bulletImage = GetComponent<Image>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (gameObject.CompareTag("Player Bullet"))
            {
                rb.MovePosition(rb.position + direction * bulletSpeed * Time.fixedDeltaTime);
            }
        }

        public void InitializePlayerBullet(Vector2 dir, float speed, Sprite sprite, float damage)
        {
            direction = dir.normalized;
            bulletSpeed = speed;
            bulletImage.sprite = sprite;
            bulletDamage = damage;
        }
        public void InitializeEnemyBullet(Vector2 dir, float speed, Sprite sprite)
        {
            rb.linearVelocity = dir.normalized * speed;
            bulletImage.sprite = sprite;
        }

    }
}

