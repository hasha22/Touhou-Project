using UnityEngine;
namespace KH
{
    public class BackgroundScroller : MonoBehaviour
    {
        [Header("Scrolling Settings")]
        [SerializeField] private float scrollSpeed = 0.5f;

        [Header("Lighting Settings")]
        [SerializeField] private Color darkColor = new Color(0.2f, 0.4f, 1f);
        [SerializeField] private Color brightColor = Color.white;

        private Material material;
        private Vector2 offset;
        private Color currentColor;

        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            material = renderer.material;
            currentColor = brightColor;
        }

        void Update()
        {
            // Handle scrolling
            offset.y += scrollSpeed * Time.deltaTime;
            material.mainTextureOffset = offset;
        }

        public void UpdateBackgroundBrightness(float faithPercentage)
        {
            Color targetColor = Color.Lerp(darkColor, brightColor, faithPercentage);
            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 2f);

            material.SetColor("_LightningColor", currentColor);
        }
    }
}