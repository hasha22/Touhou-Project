using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KH
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [Header("Right-Side UI Elements")]
        public TextMeshProUGUI currentScore;
        public TextMeshProUGUI hiScore;
        public TextMeshProUGUI powerScore;
        public TextMeshProUGUI grazes;
        [Space]
        public GameObject lifePrefab;
        [SerializeField] private Transform lifeContainer;

        [Header("Death Screen UI")]
        [SerializeField] private GameObject deathScreen;

        [Header("Playable Area UI")]
        public TextMeshProUGUI currentFaith;

        [Header("Boss UI References")]
        public TextMeshProUGUI bossNameText;
        public TextMeshProUGUI spellNameText;
        public TextMeshProUGUI phaseTimerText;
        [Space]
        public Image attackHealthBar;
        public Image spellCardHealthBar;
        [Space]
        public Transform bossLivesParent;
        public GameObject bossLifePrefab;

        [Header("Boss UI Settings")]
        public Color normalPhaseColor = Color.white;
        public Color spellPhaseColor = new Color(0.7f, 0.3f, 1f);
        public float phaseTransitionSpeed = 2f;
        private float currentHealth;
        private float maxHealth;
        public float currentPhaseDuration = 0f;
        private int currentLives;
        private bool isSpellPhase = false;

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
        private void Start()
        {
            HideBossUI();
        }
        private void Update()
        {
            if (currentPhaseDuration > 0)
            {
                currentPhaseDuration -= Time.deltaTime;
            }
            else if (currentPhaseDuration < 0)
            {
                currentPhaseDuration = 0;
            }
        }
        #region Player UI
        public void UpdateScoreUI(int score, int highScore)
        {
            if (currentScore != null)
            { currentScore.text = $"{score:D9}"; }
            if (hiScore != null)
            { hiScore.text = $"{highScore:D9}"; }
        }
        public void UpdateFaithUI(int faith)
        {
            if (currentFaith != null)
            { currentFaith.text = $"{faith}"; }
        }
        public void UpdatePowerUI(float power)
        {
            if (powerScore != null)
            { powerScore.text = $"{power:F2}"; }
        }
        public void UpdateGrazeUI(float graze)
        {
            if (grazes != null)
            { grazes.text = $"{graze}"; }
        }
        public void AddLife()
        {
            GameObject img = Instantiate(lifePrefab, lifeContainer);
        }
        public void RemoveLife()
        {
            foreach (Transform transform in lifeContainer)
            {
                Destroy(transform.gameObject);
                break;
            }
        }
        #endregion

        #region Boss UI
        public void InitializeBossUI(Boss bossData)
        {
            bossNameText.text = bossData.bossName;
            currentLives = bossData.phases.Length / 2;
            CreateBossLifeIcons();
            ShowBossUI();

        }
        public void StartBossPhase(BossPhase bossPhase)
        {
            isSpellPhase = bossPhase.isSpellCard;
            spellNameText.text = isSpellPhase ? bossPhase.phaseName : "";
            maxHealth = bossPhase.phaseBossHealth;
            currentHealth = bossPhase.phaseBossHealth;
            currentPhaseDuration = bossPhase.duration;

            attackHealthBar.fillAmount = isSpellPhase ? 0f : 1f;
            spellCardHealthBar.fillAmount = isSpellPhase ? 1f : 0f;

            attackHealthBar.gameObject.SetActive(true);
            spellCardHealthBar.gameObject.SetActive(true);
        }
        public void UpdateHealth(float newHealth)
        {
            currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
            float fill = currentHealth / maxHealth;

            if (!isSpellPhase)
            {
                attackHealthBar.fillAmount = fill;
                spellCardHealthBar.fillAmount = 1;
            }
            else
            {
                attackHealthBar.fillAmount = 0;
                spellCardHealthBar.fillAmount = fill;
            }
        }
        public void UpdateTimer()
        {
            phaseTimerText.text = currentPhaseDuration.ToString("F0");
        }
        public void OnLifeLost(int remainingLives)
        {
            currentLives = remainingLives;
            UpdateBossLifeIcons();
        }
        private void CreateBossLifeIcons()
        {
            foreach (Transform child in bossLivesParent)
                Destroy(child.gameObject);

            for (int i = 0; i < currentLives - 1; i++)
                Instantiate(bossLifePrefab, bossLivesParent);
        }
        private void UpdateBossLifeIcons()
        {
            foreach (Transform transform in bossLivesParent)
            {
                Destroy(transform.gameObject);
                break;
            }
        }
        public void ShowBossUI()
        {
            bossNameText.gameObject.SetActive(true);
            spellNameText.gameObject.SetActive(true);
            phaseTimerText.gameObject.SetActive(true);

            bossLivesParent.gameObject.SetActive(true);

            attackHealthBar.gameObject.SetActive(true);
            spellCardHealthBar.gameObject.SetActive(true);
        }
        public void HideBossUI()
        {
            bossNameText.gameObject.SetActive(false);
            spellNameText.gameObject.SetActive(false);
            phaseTimerText.gameObject.SetActive(false);

            bossLivesParent.gameObject.SetActive(false);

            attackHealthBar.gameObject.SetActive(false);
            spellCardHealthBar.gameObject.SetActive(false);
        }
        #endregion
        public void StartDeathScreenCoroutine()
        {
            StartCoroutine(EnableDeathScreen());
        }
        private IEnumerator EnableDeathScreen()
        {
            yield return new WaitForSeconds(1);
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
            AudioManager.instance.bgmSource.Stop();
        }

    }
}