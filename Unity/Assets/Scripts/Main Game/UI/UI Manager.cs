using System.Collections;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI hiScore;
    public TextMeshProUGUI powerScore;
    public GameObject lifePrefab;
    [SerializeField] private Transform lifeContainer;
    [SerializeField] private GameObject deathScreen;
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
    public void AddLife()
    {
        Instantiate(lifePrefab, lifeContainer);
    }
    public void RemoveLife()
    {
        foreach (Transform transform in lifeContainer)
        {
            Destroy(transform.gameObject);
            break;
        }
    }
    public void StartDeathScreenCoroutine()
    {
        StartCoroutine(EnableDeathScreen());
    }
    private IEnumerator EnableDeathScreen()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
        deathScreen.SetActive(true);
    }

}
