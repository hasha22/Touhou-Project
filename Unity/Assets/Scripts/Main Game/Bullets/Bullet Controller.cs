using UnityEngine;
using UnityEngine.UI;
namespace KH
{
    public class BulletController : MonoBehaviour
    {
        [Header("Bullet Behavior")]
        [SerializeField] private float bulletSpeed = 50f;
        private Vector2 direction;
        private Image bulletImage;
        private Rigidbody2D rb;
        private void Awake()
        {
            bulletImage = GetComponent<Image>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + direction * bulletSpeed * Time.fixedDeltaTime);
        }

        public void InitializeBullet(Vector2 dir, float speed, Sprite sprite)
        {
            direction = dir.normalized;
            bulletSpeed = speed;
            bulletImage.sprite = sprite;
        }

    }
}

