using UnityEngine;
namespace KH
{
    public class BulletController : MonoBehaviour
    {
        [Header("Universal Bullet Behavior")]
        public Collider2D bulletHitBox;
        [HideInInspector] public float bulletSpeed;
        private Vector2 direction;
        private Vector2 currentDirection;

        [Header("References")]
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private BulletType currentBulletType;
        private BulletGrazing bulletGrazing;

        [Header("Player Bullet")]
        [HideInInspector] public int bulletDamage;

        [Header("Deceleration Bullet Behavior")]
        private bool isDecelerating;
        private float decelerationTimer;
        private float decelerationDuration;
        private AnimationCurve decelerationCurve;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            bulletGrazing = GetComponent<BulletGrazing>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            float currentSpeed = bulletSpeed;

            if (isDecelerating)
            {
                decelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(decelerationTimer / decelerationDuration);
                float curveValue = decelerationCurve.Evaluate(t);
                currentSpeed = bulletSpeed * curveValue;

                if (t >= 1f)
                    isDecelerating = false;
            }
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
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
            //rb.linearVelocity = dir.normalized * speed;
            spriteRenderer.sprite = sprite;
            bulletSpeed = speed;
            direction = dir.normalized;

            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;

            currentDirection = direction;
            isDecelerating = false;

            if (NeedsNewColliders(bulletType))
            {
                RecreateColliders(bulletType, spriteWidth, spriteHeight);
            }
        }
        private void AddHitboxCollider(BulletType bulletType, float spriteWidth, float spriteHeight)
        {
            switch (bulletType.colliderType)
            {
                case ColliderType.Box:
                    BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();
                    boxCol.isTrigger = true;
                    boxCol.size = new Vector2(spriteWidth * bulletType.boxSize.x, spriteHeight * bulletType.boxSize.y);
                    boxCol.offset = new Vector2(spriteWidth * bulletType.boxOffset.x, spriteHeight * bulletType.boxOffset.y);
                    bulletHitBox = boxCol;
                    break;

                case ColliderType.Circle:
                    CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();
                    circleCol.isTrigger = true;
                    circleCol.radius = Mathf.Max(spriteWidth, spriteHeight) * bulletType.circleRadius;
                    circleCol.offset = new Vector2(spriteWidth * bulletType.circleOffset.x, spriteHeight * bulletType.circleOffset.y);
                    bulletHitBox = circleCol;
                    break;

                case ColliderType.Capsule:
                    CapsuleCollider2D capsuleCol = gameObject.AddComponent<CapsuleCollider2D>();
                    capsuleCol.isTrigger = true;
                    capsuleCol.size = new Vector2(spriteWidth * bulletType.capsuleSize.x, spriteHeight * bulletType.capsuleSize.y);
                    capsuleCol.offset = new Vector2(spriteWidth * bulletType.capsuleOffset.x, spriteHeight * bulletType.capsuleOffset.y);
                    capsuleCol.direction = bulletType.capsuleDirection;
                    bulletHitBox = capsuleCol;
                    break;
            }
        }
        private void AddGrazeCollider(BulletType bulletType, float spriteWidth, float spriteHeight)
        {
            switch (bulletType.grazeColliderType)
            {
                case ColliderType.Box:
                    BoxCollider2D grazeBoxCol = gameObject.AddComponent<BoxCollider2D>();
                    grazeBoxCol.size = new Vector2(spriteWidth * bulletType.grazeBoxSize.x, spriteHeight * bulletType.grazeBoxSize.y);
                    grazeBoxCol.offset = new Vector2(spriteWidth * bulletType.grazeBoxOffset.x, spriteHeight * bulletType.grazeBoxOffset.y);
                    grazeBoxCol.isTrigger = true;
                    bulletGrazing.grazeCollider = grazeBoxCol;
                    break;

                case ColliderType.Circle:
                    CircleCollider2D grazeCircleCol = gameObject.AddComponent<CircleCollider2D>();
                    grazeCircleCol.radius = Mathf.Max(spriteWidth, spriteHeight) * bulletType.grazeCircleRadius;
                    grazeCircleCol.offset = new Vector2(spriteWidth * bulletType.grazeCircleOffset.x, spriteHeight * bulletType.grazeCircleOffset.y);
                    grazeCircleCol.isTrigger = true;
                    bulletGrazing.grazeCollider = grazeCircleCol;
                    break;

                case ColliderType.Capsule:
                    CapsuleCollider2D grazeCapsuleCol = gameObject.AddComponent<CapsuleCollider2D>();
                    grazeCapsuleCol.size = new Vector2(spriteWidth * bulletType.grazeCapsuleSize.x, spriteHeight * bulletType.grazeCapsuleSize.y);
                    grazeCapsuleCol.offset = new Vector2(spriteWidth * bulletType.grazeCapsuleOffset.x, spriteHeight * bulletType.grazeCapsuleOffset.y);
                    grazeCapsuleCol.direction = bulletType.grazeCapsuleDirection;
                    grazeCapsuleCol.isTrigger = true;
                    bulletGrazing.grazeCollider = grazeCapsuleCol;
                    break;
            }
        }
        private bool NeedsNewColliders(BulletType bulletType)
        {
            return currentBulletType == null || bulletType.bulletID != currentBulletType.bulletID;
        }
        private void RecreateColliders(BulletType bulletType, float spriteWidth, float spriteHeight)
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
                DestroyImmediate(col);

            AddHitboxCollider(bulletType, spriteWidth, spriteHeight);
            AddGrazeCollider(bulletType, spriteWidth, spriteHeight);
        }
        public void StartDeceleration(AnimationCurve animationCurve, float duration)
        {
            decelerationCurve = animationCurve;
            decelerationDuration = duration;
            decelerationTimer = 0f;
            isDecelerating = true;
        }

    }
}

