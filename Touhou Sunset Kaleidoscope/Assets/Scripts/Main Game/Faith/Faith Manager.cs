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

        [Header("Decay Settings")]
        [SerializeField] private int decayRate = 250; // faith per second
        [SerializeField] private int decayDelay = 1;  // time before decay starts
        private float timeSinceLastLight = 0f;

        [Header("Visuals")]
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight;
        [SerializeField] private int faithUpdateSpeed = 200;

        [Header("Coroutines")]
        private Coroutine faithDecayCoroutine;

        [Header("Flags")]
        public bool auraActive = false;
        public bool playerInLight = false;
        public bool isPlayerDead = false;

        [Header("References")]
        private PlayerManager playerManager;
        [SerializeField] private PlayerAuraController playerAuraController;
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
                AddFaith(1000);
                isPlayerDead = false;
            }
        }
        private void UpdateFaithVisuals()
        {
            if (displayedFaith < currentFaith)
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
            else if (displayedFaith > currentFaith)
            {
                displayedFaith -= Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);
                UIManager.instance.UpdateFaithUI(displayedFaith);
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

            // smoothly adjust intensity
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, Mathf.Lerp(0.1f, 1f, t), Time.deltaTime);

            // optionally change color tone (warm for high faith, cool for low)
            Color brightColor = Color.Lerp(new Color(0.2f, 0.4f, 1f), Color.white, t);
            globalLight.color = brightColor;
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
    }
}
