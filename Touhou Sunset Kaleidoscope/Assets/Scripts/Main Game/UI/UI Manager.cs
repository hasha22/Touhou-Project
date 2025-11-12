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

        [Header("Pause Menu UI")]
        [SerializeField] private GameObject pauseMenu;
        public bool isInPauseMenu = false;

        [Header("Death Screen UI")]
        [SerializeField] private GameObject deathScreen;
        [SerializeField] private GameObject retryButton;
        [SerializeField] private bool isInDeathScreen = false;
        [SerializeField] private float delayBeforeDeathScreen = 1f;

        [Header("Victory Screen UI")]
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private GameObject playAgainButton;
        [SerializeField] private bool isInVictoryScreen = false;
        [SerializeField] private float delayBeforeVictoryScreen = 2f;

        [Header("Playable Area UI")]
        public TextMeshProUGUI currentFaith;

        [Header("Boss UI References")]
        public TextMeshProUGUI bossNameText;
        public TextMeshProUGUI phaseTimerText;
        [Space]
        public Image attackHealthBar;
        public Image spellCardHealthBar;
        [SerializeField] private float fillSpeed = 1.5f; // bar fill speed
        [Space]
        public Transform bossLivesParent;
        public GameObject bossLifePrefab;

        [Header("Boss UI Settings")]
        private float currentHealth;
        private float maxHealth;
        [HideInInspector] public float currentPhaseDuration = 0f;
        private int currentLives;
        private bool isSpellPhase = false;

        [Header("Point of Collection UI")]
        [SerializeField] private GameObject pointOfCollection;
        [SerializeField] private float flickerDuration = 3f;
        [SerializeField] private float flickerInterval = 0.2f;
        [SerializeField] private float delayBeforeFlicker = 2f;
        private CanvasGroup pocCanvasGroup;

        [Header("Boss Spell Card Cut-In")]
        [SerializeField] private RectTransform cutInTransform;
        [SerializeField] private CanvasGroup cutInGroup;
        [SerializeField] private Image bossPortraitImage;
        [SerializeField] private TextMeshProUGUI cutInSpellName;

        [Header("Spell Sign UI Target (Top Slot)")]
        [SerializeField] private RectTransform spellSignTransform;
        [SerializeField] private TextMeshProUGUI spellSignText;

        [Header("Cut-In Animation Settings")]
        [SerializeField] private float startPosX;
        [SerializeField] private float centerPosX;
        [SerializeField] private float exitPosX;
        [SerializeField] private float posY;
        [SerializeField] private float moveDuration = 0.6f;
        [SerializeField] private float holdDuration = 0.5f;
        [SerializeField] private float fadeDuration = 0.4f;
        [SerializeField] private float textMoveDuration = 0.7f;
        [SerializeField] private float transparency = 0.8f;

        [Header("Menu SFX")]
        [SerializeField] private AudioClip pauseMenuSFX;
        [SerializeField][Range(0, 1)] private float pauseMenuVolume = 1f;

        [Header("Coroutines")]
        private Coroutine fillRoutine;
        private Coroutine pointOfCollectionRoutine;

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
            pocCanvasGroup = pointOfCollection.GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            HideBossUI();
            if (pointOfCollectionRoutine != null)
            {
                StopCoroutine(pointOfCollectionRoutine);
            }
            pointOfCollectionRoutine = StartCoroutine(PointOfCollectionFlicker());
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isInPauseMenu && !isInDeathScreen && !isInVictoryScreen)
            {
                EnablePauseMenu();
                AudioManager.instance.PlaySFX(pauseMenuSFX, pauseMenu.transform, pauseMenuVolume);
            }

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
            attackHealthBar.gameObject.SetActive(true);
            spellCardHealthBar.gameObject.SetActive(true);

            if (bossPhase.isSpellCard) spellSignText.gameObject.SetActive(true);
            else spellSignText.gameObject.SetActive(false);

            spellSignText.text = "";
            isSpellPhase = bossPhase.isSpellCard;
            maxHealth = bossPhase.phaseBossHealth;
            currentHealth = bossPhase.phaseBossHealth;
            currentPhaseDuration = bossPhase.duration;

        }
        public void PlaySpellCardCutIn(Sprite bossPortrait, string spellName)
        {
            StopAllCoroutines();
            StartCoroutine(SpellCardCutInRoutine(bossPortrait, spellName));
        }
        private IEnumerator PointOfCollectionFlicker()
        {
            yield return new WaitForSeconds(delayBeforeFlicker);
            pointOfCollection.SetActive(true);
            float elapsed = 0f;
            bool visible = true;

            while (elapsed < flickerDuration)
            {
                // Toggle visibility by adjusting alpha
                visible = !visible;
                pocCanvasGroup.alpha = visible ? 1f : 0f;

                yield return new WaitForSeconds(flickerInterval);
                elapsed += flickerInterval;
            }

            // Hide completely at the end
            pocCanvasGroup.alpha = 0f;
            pointOfCollection.SetActive(false);
        }
        private IEnumerator SpellCardCutInRoutine(Sprite portrait, string spellName)
        {
            // setup
            AudioManager.instance.PlaySFX(AudioManager.instance.spellCardSFX, EnemyDatabase.instance.currentActiveBoss.transform, AudioManager.instance.spellCardSFXVolume);
            cutInGroup.alpha = 0f;
            cutInGroup.gameObject.SetActive(true);
            bossPortraitImage.sprite = portrait;
            cutInSpellName.text = spellName;
            spellSignText.text = "";

            // cut-in positions
            Vector2 startPos = new Vector2(startPosX, posY);
            Vector2 centerPos = new Vector2(centerPosX, posY);
            Vector2 exitPos = new Vector2(exitPosX, posY);

            cutInTransform.anchoredPosition = startPos;

            float timer = 0f;
            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / moveDuration);
                float eased = Mathf.SmoothStep(0f, 1f, t);

                cutInTransform.anchoredPosition = Vector2.Lerp(startPos, centerPos, eased);
                cutInGroup.alpha = Mathf.Lerp(0f, transparency, eased);
                yield return null;
            }

            cutInTransform.anchoredPosition = centerPos;

            yield return new WaitForSeconds(holdDuration);

            RectTransform cutInSpellTransform = cutInSpellName.rectTransform;
            Vector2 startAnchor = cutInSpellTransform.anchoredPosition;

            Vector3 targetWorldPos = spellSignTransform.position;
            Vector3 localTargetPos = cutInSpellTransform.parent.InverseTransformPoint(targetWorldPos);
            Vector2 targetAnchor = new Vector2(startAnchor.x, localTargetPos.y);

            timer = 0f;
            while (timer < textMoveDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / textMoveDuration);
                float eased = EaseOutCubic(t);
                cutInSpellTransform.anchoredPosition = Vector2.LerpUnclamped(startAnchor, targetAnchor, eased);
                yield return null;
            }

            cutInSpellTransform.anchoredPosition = targetAnchor;

            spellSignText.text = spellName;
            cutInSpellName.text = "";

            // reset cut-in text for next time
            cutInSpellTransform.anchoredPosition = startAnchor;

            // fade-out
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeDuration);
                cutInGroup.alpha = Mathf.Lerp(transparency, 0f, t);
                yield return null;
            }

            cutInGroup.gameObject.SetActive(false);
        }
        public void InitializeHealth(float current, float max)
        {
            float targetFill = Mathf.Clamp01(current / max);

            attackHealthBar.fillAmount = 0f;
            spellCardHealthBar.fillAmount = 0f;

            if (fillRoutine != null) StopCoroutine(fillRoutine);

            fillRoutine = StartCoroutine(FillHealthBarsSequential(targetFill));
        }
        private IEnumerator FillHealthBarsSequential(float targetFill)
        {
            float spellCardFill = spellCardHealthBar.fillAmount;
            float attackFill = attackHealthBar.fillAmount;

            yield return FillBar(spellCardHealthBar, targetFill);
            yield return FillBar(attackHealthBar, targetFill);

            spellCardHealthBar.fillAmount = targetFill;
            attackHealthBar.fillAmount = targetFill;

            fillRoutine = null;
        }
        private IEnumerator FillBar(Image bar, float targetFill)
        {
            float start = bar.fillAmount;
            float t = 0f;

            // If already at or above target, skip
            if (Mathf.Approximately(start, targetFill) || start > targetFill)
            {
                bar.fillAmount = targetFill;
                yield break;
            }

            while (t < 1f)
            {
                t += Time.deltaTime * fillSpeed;
                float fill = Mathf.Lerp(start, targetFill, t);
                bar.fillAmount = fill;
                yield return null;
            }

            bar.fillAmount = targetFill;
        }
        public void UpdateHealthImmediate(float newHealth)
        {
            float fill = Mathf.Clamp01(newHealth / maxHealth);

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
            phaseTimerText.gameObject.SetActive(true);

            bossLivesParent.gameObject.SetActive(true);

            attackHealthBar.gameObject.SetActive(true);
            spellCardHealthBar.gameObject.SetActive(true);
        }
        public void HideBossUI()
        {
            bossNameText.gameObject.SetActive(false);
            spellSignText.gameObject.SetActive(false);
            phaseTimerText.gameObject.SetActive(false);

            bossLivesParent.gameObject.SetActive(false);

            attackHealthBar.gameObject.SetActive(false);
            spellCardHealthBar.gameObject.SetActive(false);
        }
        public void StartVictoryScreenCoroutine()
        {
            StartCoroutine(EnableVictoryScreen());
        }
        #endregion
        public void StartDeathScreenCoroutine()
        {
            StartCoroutine(EnableDeathScreen());
        }
        private IEnumerator EnableVictoryScreen()
        {
            yield return new WaitForSeconds(delayBeforeVictoryScreen);
            isInVictoryScreen = true;
            Time.timeScale = 0f;
            victoryScreen.SetActive(true);
            AudioManager.instance.bgmSource.Stop();
        }
        private IEnumerator EnableDeathScreen()
        {
            yield return new WaitForSeconds(delayBeforeDeathScreen);
            isInDeathScreen = true;
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
            AudioManager.instance.bgmSource.Stop();
        }
        public void EnablePauseMenu()
        {
            isInPauseMenu = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            AudioManager.instance.bgmSource.Pause();
        }
        public void DisablePauseMenu()
        {
            isInPauseMenu = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            AudioManager.instance.bgmSource.UnPause();
        }
        public void OnRetryButtonPressed()
        {
            Time.timeScale = 1f;
            deathScreen.SetActive(false);
            victoryScreen.SetActive(false);
            isInDeathScreen = false;

            attackHealthBar.fillAmount = 1f;
            spellCardHealthBar.fillAmount = 1f;

            ScoreManager.instance.ResetScore();
            EnemyDatabase.instance.ClearAllEnemies();
            LightZoneManager.instance.RemoveAllZones();
            ItemManager.instance.RemoveAllItems();
            StageManager.instance.ResetStage();
            FaithManager.instance.ResetFaith();
            AudioManager.instance.ResetAudioManager();
            ObjectPool.instance.RemoveAllBullets();
            HideBossUI();
            if (pointOfCollectionRoutine != null)
            {
                StopCoroutine(pointOfCollectionRoutine);
            }
            pointOfCollectionRoutine = StartCoroutine(PointOfCollectionFlicker());

            PlayerManager player = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            player.ResetPlayer();
        }
        private float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    }
}