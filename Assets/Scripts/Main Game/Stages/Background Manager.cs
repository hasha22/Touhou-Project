using KH;
using System.Collections;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance { get; private set; }

    [Header("Background")]
    public Renderer backgroundRenderer;
    public float transitionDuration = 2f;
    public BackgroundScroller backgroundScroller;

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
    public void SwitchBackground(Material newMaterial)
    {
        if (newMaterial == backgroundRenderer.material) return;

        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(newMaterial));
    }
    private IEnumerator FadeCoroutine(Material newMaterial)
    {
        backgroundRenderer.material = newMaterial;

        Color color = backgroundRenderer.material.GetColor("_Base_Color");
        color.a = 0f;
        backgroundRenderer.material.SetColor("_Base_Color", color);

        float t = 0f;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / transitionDuration);

            color.a = lerp;
            backgroundRenderer.material.SetColor("_Base_Color", color);

            yield return null;
        }

        color.a = 1f;
        backgroundRenderer.material.SetColor("_Base_Color", color);
        backgroundScroller.RefreshMaterial();
    }
}
