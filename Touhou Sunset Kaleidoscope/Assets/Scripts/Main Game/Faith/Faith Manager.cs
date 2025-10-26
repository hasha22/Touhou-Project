using UnityEngine;
namespace KH
{
    public class FaithManager : MonoBehaviour
    {
        public static FaithManager instance { get; private set; }

        [Header("Faith Values")]
        public int currentFaith = 5000;
        [SerializeField] private int maxFaith = 10000;
        [SerializeField] private int minFaith = 0;

        [Header("Decay Settings")]
        [SerializeField] private int decayRate = 250; // faith per second
        [SerializeField] private int decayDelay = 1;  // time before decay starts
        private float timeSinceLastLight = 0f;

        [Header("Visuals")]
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight;

        [Header("Thresholds")]
        [SerializeField] private int auraThreshold = 8000;

        private bool auraActive = false;
        private bool playerInLight = false;
        private void Awake()
        {
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
            if (Input.GetKey(KeyCode.W))
                AddFaith(500);
            if (Input.GetKey(KeyCode.S))
                RemoveFaith(500);
            UpdateLightningVisuals();
            UpdateFaithDecay();
        }
        public void AddFaith(int amount)
        {
            //+= Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);

            currentFaith = Mathf.Clamp(currentFaith + amount, minFaith, maxFaith);
            UIManager.instance.UpdateFaithUI(currentFaith);
            timeSinceLastLight = 0f;
            CheckThreshold();
        }
        public void RemoveFaith(int amount)
        {
            currentFaith = Mathf.Clamp(currentFaith - amount, minFaith, maxFaith);
            if (currentFaith <= 0) currentFaith = minFaith;
            UIManager.instance.UpdateFaithUI(currentFaith);
            CheckThreshold();
        }
        private void UpdateFaithDecay()
        {
            if (!playerInLight)
                timeSinceLastLight += Time.deltaTime;
            else
                timeSinceLastLight = 0f;

            if (timeSinceLastLight > decayDelay)
            {
                RemoveFaith(decayRate);
            }

            if (currentFaith <= 0)
            {
                // handle death to darkness here
            }
        }
        private void UpdateLightningVisuals()
        {
            float t = currentFaith / maxFaith;

            // smoothly adjust intensity
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, Mathf.Lerp(0.1f, 1f, t), Time.deltaTime * 2f);
            //globalLight.intensity = Mathf.Lerp(0.1f, 1f, t);

            // optionally change color tone (warm for high faith, cool for low)
            Color brightColor = Color.Lerp(new Color(0.2f, 0.4f, 1f), Color.white, t);
            globalLight.color = brightColor;
        }
        private void CheckThreshold()
        {
            if (currentFaith >= auraThreshold && !auraActive)
            {
                auraActive = true;
            }
            else if (currentFaith < auraThreshold)
            {
                auraActive = false;
            }
        }
    }
}
