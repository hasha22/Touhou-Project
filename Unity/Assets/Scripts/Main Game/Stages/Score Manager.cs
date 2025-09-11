using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score Variables")]
    public int CurrentScore { get; private set; }
    public int HiScore { get; private set; }

    private int displayedCurrentScore = 0;
    private int displayedHiScore = 0;
    [SerializeField] private float scoreUpdateSpeed = 5000f;

    [Header("Save System")]
    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = new SaveSystem();

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
    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
