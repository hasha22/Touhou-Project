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
        public SpriteRenderer gradientVisual;
        private float auraTimer;
        private bool auraActive = false;
        private Coroutine auraFadeCoroutine;
        private Transform playerTransform;
        private void Start()
        {
            playerTransform = PlayerInputManager.instance.playerObject.transform;
            auraVisual.enabled = false;
            gradientVisual.enabled = false;
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
            gradientVisual.enabled = true;
            auraFadeCoroutine = StartCoroutine(FadeAura(true));
        }
        private void DeactivateAura()
        {
            auraActive = false;
            if (auraFadeCoroutine != null)
                StopCoroutine(auraFadeCoroutine);

            auraFadeCoroutine = StartCoroutine(FadeAura(false));
        }
        private IEnumerator FadeAura(bool fadeIn)
        {
            float duration = 0.75f;
            float elapsed = 0f;
            UnityEngine.Color auraColor = auraVisual.color;
            UnityEngine.Color gradientColor = gradientVisual.color;

            float startAlpha = fadeIn ? 0f : 1f;
            float targetAlpha = fadeIn ? 1f : 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                float alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, t));

                auraVisual.color = new UnityEngine.Color(auraColor.r, auraColor.g, auraColor.b, alpha);
                gradientVisual.color = new UnityEngine.Color(gradientColor.r, gradientColor.g, gradientColor.b, alpha);


                yield return null;
            }

            auraVisual.color = new UnityEngine.Color(auraColor.r, auraColor.g, auraColor.b, targetAlpha);
            gradientVisual.color = new UnityEngine.Color(gradientColor.r, gradientColor.g, gradientColor.b, targetAlpha);

            if (!fadeIn)
            {
                auraVisual.enabled = false;
                gradientVisual.enabled = false;
            }
        }
    }
}


