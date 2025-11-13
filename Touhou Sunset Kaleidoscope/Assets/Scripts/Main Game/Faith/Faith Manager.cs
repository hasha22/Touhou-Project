using System.Collections;
using UnityEngine;
namespace KH
{
    public class FaithManager : MonoBehaviour
    {
        public static FaithManager instance { get; private set; }

        [Header("Faith Values")]
        public int currentFaith = 5000;
        [HideInInspector] public int displayedFaith;
        [SerializeField] private int maxFaith = 10000;
        [SerializeField] private int minFaith = 0;
        [SerializeField] private int respawnFaith = 5000;

        [Header("Decay Settings")]
        [SerializeField] private int decayRate = 250; // faith per second
        [SerializeField] private int decayDelay = 1;  // time before decay starts
        private float timeSinceLastLight = 0f;

        [Header("Visuals")]
        [SerializeField] private Renderer bgRenderer;
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight;
        [SerializeField] private int faithUpdateSpeed = 200;

        [Header("Coroutines")]
        private Coroutine faithDecayCoroutine;

        [Header("Flags")]
        public bool auraActive = false;
        public bool playerInLight = false;
        public bool isPlayerDead = false;
        public bool isPaused = false;
        public bool isRestarted = false;

        [Header("References")]
        private PlayerManager playerManager;
        [SerializeField] private PlayerAuraController playerAuraController;
        [SerializeField] private BackgroundScroller backgroundScroller;
        private void Awake()
        {
            displayedFaith = currentFaith;
            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            //testing
            if (Input.GetKey(KeyCode.W))
                AddFaith(100);

            if (isPaused)
            {
                if (faithDecayCoroutine != null)
                { StopCoroutine(faithDecayCoroutine); }
                return;
            }

            UpdateFaithDecay();
            UpdateLightningVisuals();
            UpdateFaithVisuals();
        }
        public void AddFaith(int amount)
        {
            currentFaith += amount;
            if (currentFaith >= maxFaith) currentFaith = maxFaith;

            timeSinceLastLight = 0f;
            CheckThreshold();
        }
        public void RemoveFaith(int amount)
        {
            currentFaith -= amount;
            if (currentFaith <= 0) currentFaith = minFaith;

            CheckThreshold();
        }
        private void UpdateFaithDecay()
        {
            playerInLight = playerManager.inLight;

            if (!playerInLight)
                timeSinceLastLight += Time.deltaTime;
            else
                timeSinceLastLight = 0f;

            if (playerInLight && faithDecayCoroutine != null)
            {
                StopCoroutine(faithDecayCoroutine);
                faithDecayCoroutine = null;
            }

            if (timeSinceLastLight > decayDelay && faithDecayCoroutine == null)
            {
                faithDecayCoroutine = StartCoroutine(FaithDecayRoutine());
            }

            // handle death to darkness here
            if (currentFaith <= 0 && !isPlayerDead)
            {
                isPlayerDead = true;
                playerManager.Die();
                AddFaith(respawnFaith);
                timeSinceLastLight = 1;
                isPlayerDead = false;
            }
        }
        private void UpdateFaithVisuals()
        {
            if (displayedFaith < currentFaith)
            {
                if (!isRestarted)
                {
                    displayedFaith += Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);

                    if (displayedFaith > currentFaith) { displayedFaith = currentFaith; }

                    UIManager.instance.UpdateFaithUI(displayedFaith);

                    if (faithDecayCoroutine != null)
                    {
                        StopCoroutine(faithDecayCoroutine);
                        faithDecayCoroutine = null;
                    }
                }
                else
                {
                    if (displayedFaith > currentFaith) { displayedFaith = currentFaith; }

                    UIManager.instance.UpdateFaithUI(displayedFaith);

                    if (faithDecayCoroutine != null)
                    {
                        StopCoroutine(faithDecayCoroutine);
                        faithDecayCoroutine = null;
                    }
                    isRestarted = false;
                }
            }
            else if (displayedFaith > currentFaith)
            {
                if (!isRestarted)
                {
                    displayedFaith -= Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);
                    UIManager.instance.UpdateFaithUI(displayedFaith);
                }
                else
                {
                    UIManager.instance.UpdateFaithUI(displayedFaith);
                    isRestarted = false;
                }
            }
        }
        private IEnumerator FaithDecayRoutine()
        {
            while (currentFaith > minFaith)
            {
                RemoveFaith(Mathf.CeilToInt(decayRate * Time.deltaTime));
                yield return null;
            }

            faithDecayCoroutine = null;
        }
        private void UpdateLightningVisuals()
        {
            float t = (float)currentFaith / maxFaith;

            // Update global light
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, Mathf.Lerp(0.1f, 1f, t), Time.deltaTime);
            Color brightColor = Color.Lerp(new Color(0.2f, 0.4f, 1f), Color.white, t);
            globalLight.color = brightColor;

            backgroundScroller.UpdateBackgroundBrightness(t);
        }
        private void CheckThreshold()
        {
            if (currentFaith >= playerAuraController.auraActivationThreshold && !auraActive)
            {
                auraActive = true;
            }
            else if (currentFaith < playerAuraController.auraActivationThreshold)
            {
                auraActive = false;
            }
        }
        public void ResetFaith()
        {
            currentFaith = 10000;
            displayedFaith = currentFaith;
            timeSinceLastLight = 2;
            faithDecayCoroutine = null;
            auraActive = false;
            playerInLight = false;
            isPlayerDead = false;
            isRestarted = true;
        }
    }
}
