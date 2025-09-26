using System.Collections;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private PlayerManager playerManager;

    [Header("Score Variables")]
    public int CurrentScore;
    public int HiScore;
    [SerializeField] private float scoreUpdateSpeed = 5000f;
    private int displayedCurrentScore = 0;
    private int displayedHiScore = 0;

    [Header("Faith Variables")]
    public int Faith;
    [SerializeField] private float faithUpdateSpeed = 5000f;
    [SerializeField] private int minFaith = 50000;
    [SerializeField] private int faithDecreaseAmount = 600;
    [SerializeField] private float faithDecreaseInterval = 1f;
    public int displayedFaith = 50000;

    [Header("Score Thresholds")]
    private bool passedFirst = false; // 20 million
    private bool passedSecond = false; // 40 million
    private bool passedThird = false; // 80 million
    private bool passedFourth = false; // 150 million

    [Header("Save System")]
    private SaveSystem saveSystem;

    [Header("Coroutines")]
    private Coroutine faithDecreaseCoroutine;

    private void Awake()
    {
        saveSystem = new SaveSystem();
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
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
        Faith = 50000;
        displayedHiScore = saveSystem.LoadScore();
        UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
    }
    private void Update()
    {
        // Score & Faith UI Updates
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

        if (displayedFaith < Faith)
        {
            displayedFaith += Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);

            if (displayedFaith > Faith) { displayedFaith = Faith; }

            UIManager.instance.UpdateFaithUI(displayedFaith);

            if (faithDecreaseCoroutine != null)
            {
                StopCoroutine(faithDecreaseCoroutine);
                faithDecreaseCoroutine = null;
            }
        }
        else if (displayedFaith > Faith)
        {
            displayedFaith -= Mathf.CeilToInt(faithUpdateSpeed * Time.deltaTime);
            UIManager.instance.UpdateFaithUI(displayedFaith);
        }
        else
        {
            if (Faith > minFaith && faithDecreaseCoroutine == null)
            {
                faithDecreaseCoroutine = StartCoroutine(FaithDecayRoutine());
            }
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
        CurrentScore += score;

        if (CurrentScore >= HiScore)
        {
            HiScore = CurrentScore;
            saveSystem.SaveScore(HiScore);
        }
        UIManager.instance.UpdateScoreUI(displayedCurrentScore, displayedHiScore);
    }
    public void AddFaith(int faith)
    {
        Faith += faith;
        UIManager.instance.UpdateFaithUI(displayedFaith);
    }
    public void DecreaseFaith(int faith)
    {
        Faith -= faith;
        if (Faith < minFaith) { Faith = minFaith; }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(faithDecreaseInterval);

        faithDecreaseCoroutine = null;
    }
    private IEnumerator FaithDecayRoutine()
    {
        yield return new WaitForSeconds(faithDecreaseInterval);
        while (Faith > minFaith)
        {
            DecreaseFaith(faithDecreaseAmount);
        }

        faithDecreaseCoroutine = null;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
