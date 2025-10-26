using UnityEngine;
using UnityEngine.UI;
namespace KH
{
    public class FaithUIBar : MonoBehaviour
    {
        [Header("Slider Setup")]
        public Slider faithSlider;
        public float sliderSpeed = 50f;
        private int minFaith = 0;
        private int maxFaith = 10000;

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
            int currentFaith = FaithManager.instance.currentFaith;

            bool shouldShow = currentFaith > minFaith;

            if (faithSlider.gameObject.activeSelf != shouldShow)
                faithSlider.gameObject.SetActive(shouldShow);

            if (currentFaith > minFaith)
            {
                faithSlider.value = Mathf.MoveTowards(faithSlider.value, currentFaith, sliderSpeed * Time.deltaTime);
            }
            else if (currentFaith >= maxFaith)
            {
                faithSlider.value = faithSlider.maxValue;
            }

        }
    }
}