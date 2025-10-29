using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class PlayerAuraController : MonoBehaviour
    {
        [Header("Aura Settings")]
        public float bulletSlowMultiplier = 0.5f;
        public float updateInterval = 0.1f;
        public float auraActivationThreshold = 8000f;
        public List<BulletController> bulletsBeingSlowed;

        [Header("References")]
        public SpriteRenderer auraVisual;
        private float auraTimer;
        private bool auraActive = false;
        private Coroutine auraFadeCoroutine;
        private Transform playerTransform;
        private void Start()
        {
            playerTransform = PlayerInputManager.instance.playerObject.transform;
            auraVisual.enabled = false;
        }

        private void Update()
        {
            bool shouldBeActive = FaithManager.instance.currentFaith >= auraActivationThreshold;

            if (shouldBeActive && !auraActive)
            {
                ActivateAura();
            }
            else if (!shouldBeActive && auraActive)
            {
                DeactivateAura();
            }

            if (auraActive)
            {
                auraTimer += Time.deltaTime;
                if (auraTimer >= updateInterval)
                {
                    auraTimer = 0f;
                }
            }
        }
        private void LateUpdate()
        {
            transform.position = playerTransform.position;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (auraActive)
            {
                if (collision.CompareTag("Enemy Bullet"))
                {
                    BulletController bulletController = collision.GetComponent<BulletController>();
                    bulletController.ApplySpeedMultiplier(bulletSlowMultiplier);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (auraActive)
            {
                if (collision.CompareTag("Enemy Bullet"))
                {
                    BulletController bulletController = collision.GetComponent<BulletController>();
                    bulletController.ResetSpeed();
                }
            }
        }
        private void ActivateAura()
        {
            auraActive = true;
            if (auraFadeCoroutine != null)
                StopCoroutine(auraFadeCoroutine);


            auraVisual.enabled = true;
            auraFadeCoroutine = StartCoroutine(FadeAura(true));
        }
        private void DeactivateAura()
        {
            auraActive = false;
            if (auraFadeCoroutine != null)
                StopCoroutine(auraFadeCoroutine);

            auraFadeCoroutine = StartCoroutine(FadeAura(false));

            // add coroutine for fading out here and resetting bullet speed
        }
        private IEnumerator FadeAura(bool fadeIn)
        {
            float duration = 0.75f;
            float elapsed = 0f;
            UnityEngine.Color color = auraVisual.color;
            //float faithFactor = Mathf.InverseLerp(8000, 10000, FaithManager.instance.currentFaith);

            float startAlpha = fadeIn ? 0f : 1f;
            float targetAlpha = fadeIn ? 1f : 0f;
            //float startScale = fadeIn ? 0.8f : 1f;
            //float targetScale = fadeIn ? 1f : 0.8f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                float alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, t));
                //float scale = Mathf.Lerp(startScale, targetScale, Mathf.SmoothStep(0f, 1f, t));

                auraVisual.color = new UnityEngine.Color(color.r, color.g, color.b, alpha);

                //auraVisual.color = new Color(color.r, color.g, color.b, alpha);
                //transform.localScale = Vector3.one * scale;

                yield return null;
            }

            auraVisual.color = new UnityEngine.Color(color.r, color.g, color.b, targetAlpha);

            if (!fadeIn)
                auraVisual.enabled = false;
        }
    }
}


