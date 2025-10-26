using System.Collections;
using UnityEngine;
namespace KH
{
    public class LightZone_Pillar : LightZoneBase
    {
        [Header("Settings")]
        public float fadeInDuration = 1.5f;
        public float fadeOutDuration = 2f;
        public float fallSpeed = 3f;
        private Vector2 direction;

        [Header("References")]
        private Coroutine alphaCoroutine;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;

        protected override void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            direction = Vector2.zero;
        }

        protected override void Update()
        {
            base.Update();

            Vector2 pos = rb.position + direction * fallSpeed * Time.fixedDeltaTime;
            rb.MovePosition(pos);
        }
        public override void Initialize(Vector2 position, LightZoneSize zoneSize)
        {
            SetNewSize(zoneSize);

            spriteRenderer = GetComponent<SpriteRenderer>();
            zoneCollider = GetComponent<Collider2D>();

            SetAlpha(0f);

            // start the fade loop
            if (alphaCoroutine != null) StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(FadeCoroutine());

            transform.position = position;
        }
        private IEnumerator FadeCoroutine()
        {
            float elapsed = 0f;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 0.5f, elapsed / fadeInDuration);
                SetAlpha(alpha);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / fadeOutDuration);
                SetAlpha(alpha);
                yield return null;
            }

            ObjectPool.instance.ReturnToPool(gameObject);
            alphaCoroutine = null;
        }
        public void SetDirection(Vector2 newDirection)
        {

            // convention: if newDirection is set to (0,0), which means it's not moving, it means the light is supposed to go towards the player's position
            // pillar light zones are never supposed to stand still

            Vector3 playerPosition = PlayerInputManager.instance.playerObject.transform.position;
            Vector2 directionToPlayer = (playerPosition - transform.position).normalized;

            direction = newDirection == Vector2.zero ? directionToPlayer : newDirection.normalized;

            // rotation
            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle - 90);
        }
        private void SetNewSize(LightZoneSize zoneSize)
        {
            switch (zoneSize)
            {
                case LightZoneSize.Large:
                    transform.localScale = new Vector3(0.3f, 1.5f, 1);
                    break;
                case LightZoneSize.Medium:
                    transform.localScale = new Vector3(0.3f, 1.2f, 1);
                    break;
                case LightZoneSize.Small:
                    transform.localScale = new Vector3(0.2f, 1f, 1);
                    break;
            }
        }
        private void SetAlpha(float alpha)
        {
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }
        }
    }
}
