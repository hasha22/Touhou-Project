using UnityEngine;
namespace KH
{
    public class Afterimage : MonoBehaviour
    {
        [Header("Afterimage Setup")]
        public float yOffset = 0.2f;
        private float afterImageSpeed;
        private Vector2 direction;
        private Transform targetBullet;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + direction * afterImageSpeed * Time.fixedDeltaTime);
        }
        public void InitializeAfterImage(Vector2 dir, float speed, Sprite sprite, Vector2 position, Vector2 playerVelocity)
        {
            transform.position = new Vector2(position.x, position.y - yOffset);
            direction = dir.normalized;
            afterImageSpeed = speed;
            spriteRenderer.sprite = sprite;

            //rb.linearVelocity = (direction.normalized * speed) - new Vector2(0f, playerVelocity.y);
        }
    }
}
