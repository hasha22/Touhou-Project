using UnityEngine;
namespace KH
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [Header("Score Variables")]
        public int CurrentScore;
        public int HiScore;
        [SerializeField] private float scoreUpdateSpeed = 5000f;
        private int displayedCurrentScore = 0;
        private int displayedHiScore = 0;

        [Header("Score Thresholds")]
        private bool passedFirst = false; // 20 million
        private bool passedSecond = false; // 40 million
        private bool passedThird = false; // 80 million
        private bool passedFourth = false; // 150 million

        [Header("Grazing")]
        [SerializeField] private int grazeCount = 0;

        [Header("References")]
        private SaveSystem saveSystem;
        private PlayerManager playerManager;

        [Header("Coroutines")]
        private Coroutine faithDecreaseCoroutine;

        private void Awake()
        {
            saveSystem = new SaveSystem();
            grazeCount = 0;
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
        private void Start()
        {
            displayedHiScore = saveSystem.LoadScore();
            UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
        }
        private void Update()
        {
            // Score UI Updates
            if (displayedCurrentScore < CurrentScore)
            {
                displayedCurrentScore += Mathf.CeilToInt(scoreUpdateSpeed * Time.deltaTime);

                if (displayedCurrentScore > CurrentScore) { displayedCurrentScore = CurrentScore; }

                UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
            }
            if (displayedHiScore < HiScore)
            {
                displayedHiScore += Mathf.CeilToInt(scoreUpdateSpeed * Time.deltaTime);

                if (displayedHiScore > HiScore) { displayedHiScore = HiScore; }

                UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
            }

            // Life Updates
            if (CurrentScore >= 20000000 && !passedFirst)
            {
                playerManager.AddLife();
                passedFirst = true;
            }
            if (CurrentScore >= 40000000 && !passedSecond)
            {
                playerManager.AddLife();
                passedSecond = true;
            }
            if (CurrentScore >= 80000000 && !passedThird)
            {
                playerManager.AddLife();
                passedThird = true;
            }
            if (CurrentScore >= 150000000 && !passedFourth)
            {
                playerManager.AddLife();
                passedFourth = true;
            }

        }
        public void AddScore(int score)
        {
            int grazeMultiplier = GetAdjustedPointItemValue();
            int lightMultiplier = playerManager.scoreMultiplier;
            int finalScore = (score * lightMultiplier) + grazeMultiplier;

            CurrentScore += finalScore;

            if (CurrentScore >= HiScore)
            {
                HiScore = CurrentScore;
                saveSystem.SaveScore(HiScore);
            }
            UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
        }
        /*
        private IEnumerator FaithDecayRoutine()
        {
            yield return new WaitForSeconds(faithDecreaseInterval);
            while (Faith > minFaith)
            {
                DecreaseFaith(faithDecreaseAmount);
            }

            faithDecreaseCoroutine = null;
        }*/
        public void RegisterGraze()
        {
            grazeCount++;
            UIManager.instance.UpdateGrazeUI(grazeCount);
        }
        public int GetAdjustedPointItemValue()
        {
            // Each 3 grazes adds 10 points to score items. tweak later
            return (grazeCount / 3) * 10;
        }
        public void ResetGrazes()
        {
            grazeCount = 0;
        }
        public int GetGrazeCount()
        {
            return grazeCount;
        }
    }
}