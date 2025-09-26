using UnityEngine;
using UnityEngine.UI;
public class FaithUIBar : MonoBehaviour
{
    [Header("Slider Setup")]
    public Slider faithSlider;
    public float depletionSpeed = 50f; // speed the bar moves visually
    private int minFaith = 50000;
    private int maxFaith = 100000;

    private void Awake()
    {
        faithSlider = GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        faithSlider.minValue = minFaith;
        faithSlider.maxValue = maxFaith;
        faithSlider.gameObject.SetActive(false);
    }
    private void Update()
    {
        int currentFaith = ScoreManager.instance.displayedFaith;

        bool shouldShow = currentFaith > minFaith;

        if (faithSlider.gameObject.activeSelf != shouldShow)
            faithSlider.gameObject.SetActive(shouldShow);

        if (currentFaith > minFaith)
        {
            faithSlider.value = Mathf.MoveTowards(faithSlider.value, currentFaith, depletionSpeed * Time.deltaTime);
        }

    }
}
