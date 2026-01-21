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
        /*
        Texture oldTexture = backgroundRenderer.material.GetTexture("_MainTex");
        backgroundRenderer.material.SetTexture("_BlendTex", newTexture);

        float t = 0f;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            backgroundRenderer.material.SetFloat("_Blend", t);
            yield return null;
        }

        backgroundRenderer.material.SetTexture("_MainTex", newTexture);
        backgroundRenderer.material.SetFloat("_Blend", 0f);
        */

        backgroundRenderer.material = newMaterial;

        Color color = backgroundRenderer.material.GetColor("_LightningColor");
        color.a = 0f;
        backgroundRenderer.material.SetColor("_LightningColor", color);

        float t = 0f;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / transitionDuration);

            color.a = lerp;
            backgroundRenderer.material.SetColor("_LightningColor", color);

            yield return null;
        }

        color.a = 1f;
        backgroundRenderer.material.SetColor("_LightningColor", color);
        backgroundScroller.RefreshMaterial();
    }
}
