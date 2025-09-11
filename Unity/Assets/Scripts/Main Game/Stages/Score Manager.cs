using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score Variables")]
    public int CurrentScore { get; private set; }
    public int HiScore { get; private set; }

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
        HiScore = saveSystem.LoadScore();
        UIManager.instance.UpdateScoreUI(CurrentScore, HiScore);
    }
    public void AddScore(int score)
    {
        CurrentScore += score;

        if (CurrentScore >= HiScore)
        {
            HiScore = CurrentScore;
            saveSystem.SaveScore(HiScore);
        }
        UIManager.instance.UpdateScoreUI(CurrentScore, HiScore);
    }
    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
