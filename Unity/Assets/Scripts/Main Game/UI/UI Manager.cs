using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI hiScore;
    public TextMeshProUGUI powerScore;
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

    public void UpdateScoreUI(int score, int highScore)
    {
        if (currentScore != null)
        { currentScore.text = $"{score:D9}"; }
        if (hiScore != null)
        { hiScore.text = $"{highScore:D9}"; }
    }
    public void UpdatePowerUI(float power)
    {
        if (powerScore != null)
        { powerScore.text = $"{power:F2}"; }
    }

}
