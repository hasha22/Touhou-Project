using System.Collections;
using TMPro;
using UnityEngine;

public class SpellSignUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform textRect;
    [SerializeField] private TextMeshProUGUI attackNameText;
    [SerializeField] private GameObject playableArea;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    [Header("Settings")]
    [SerializeField] private float rightEdgeOffset = 20f; // top and right edge padding
    [SerializeField] private float topEdgeOffset = 20f;
    [SerializeField] private float slideDistance = 50f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float paddingHorizontal = 20f; // padding inside the rect
    [SerializeField] private float minWidth = 100f;
    [SerializeField] private float maxWidth = 500f;

    private void Start()
    {
        mainCamera = Camera.main;

        BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
        Bounds bounds = area.bounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
    }
    public void SetAttackName(string newName, bool isSpellCard)
    {
        if (isSpellCard) return;

        attackNameText.text = newName;
        attackNameText.ForceMeshUpdate();

        // gets preferred text width 
        float textWidth = attackNameText.preferredWidth;

        // calculates rect width
        float rectWidth = Mathf.Clamp(textWidth + paddingHorizontal, minWidth, maxWidth);

        // updates text rect transform width
        textRect.sizeDelta = new Vector2(rectWidth, textRect.sizeDelta.y);

        // convert world bounds to screen space
        Vector2 topRightWorld = new Vector2(maxBounds.x - rightEdgeOffset, maxBounds.y - topEdgeOffset);
        Vector2 topLeftWorld = new Vector2(minBounds.x, maxBounds.y - topEdgeOffset);

        // converts screen space to canvas space
        Vector2 topRightCanvas = WorldToCanvasPosition(topRightWorld);
        Vector2 topLeftCanvas = WorldToCanvasPosition(topLeftWorld);

        // calculates position from right edge
        float xPosition = topRightCanvas.x - rectWidth;
        float yPosition = topRightCanvas.y;

        // sets new position
        if (xPosition < topLeftCanvas.x)
        {
            xPosition = topLeftCanvas.x;
        }

        textRect.anchoredPosition = new Vector2(xPosition, yPosition);

        StartCoroutine(SlideInName());

    }
    private Vector2 WorldToCanvasPosition(Vector2 worldPosition)
    {
        // Convert world position to screen position
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out Vector2 canvasPosition
        );

        return canvasPosition;
    }

    private IEnumerator SlideInName()
    {
        Vector2 targetPos = textRect.anchoredPosition;
        Vector2 startPos = targetPos - new Vector2(0, slideDistance);

        float timer = 0f;
        while (timer < slideDuration)
        {
            timer += Time.deltaTime;
            float t = slideCurve.Evaluate(timer / slideDuration);
            textRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure final position is exact
        textRect.anchoredPosition = targetPos;

    }
}
