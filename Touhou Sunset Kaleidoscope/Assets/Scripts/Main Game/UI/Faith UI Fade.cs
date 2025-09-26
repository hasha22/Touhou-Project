using UnityEngine;
public class FaithUIFade : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float fadedAlpha = 0.1f;
    private float targetAlpha = 1f;

    [Header("References")]
    private CanvasGroup canvasGroup;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform uiElement;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(player.position);

        if (RectTransformUtility.RectangleContainsScreenPoint(uiElement, screenPoint))
            targetAlpha = fadedAlpha;
        else
            targetAlpha = 1f;

        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }
}
