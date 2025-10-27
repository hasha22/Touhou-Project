using System.Collections;
using UnityEngine;
namespace KH
{
    public class LightZone_Circular : LightZoneBase
    {
        [Header("Settings")]
        public float fadeInDuration = 1.5f;
        public float fadeOutDuration = 2f;

        [Header("References")]
        private Coroutine alphaCoroutine;
        private SpriteRenderer spriteRenderer;
        private void OnDisable()
        {
            if (alphaCoroutine != null)
                StopCoroutine(alphaCoroutine);

            alphaCoroutine = null;
        }
        public override void Initialize(Vector2 position, LightZoneSize zoneSize)
        {
            SetNewSize(zoneSize);
            transform.position = position;
            gameObject.SetActive(true);

            spriteRenderer = GetComponent<SpriteRenderer>();
            zoneCollider = GetComponent<Collider2D>();

            SetAlpha(0f);

            // start the fade loop
            if (alphaCoroutine != null) StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(FadeCoroutine());

        }
        private void SetNewSize(LightZoneSize zoneSize)
        {
            switch (zoneSize)
            {
                case LightZoneSize.Large:
                    transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    break;
                case LightZoneSize.Medium:
                    transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    break;
                case LightZoneSize.Small:
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                    break;
            }
        }
        protected override void Update()
        {
            base.Update();
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
