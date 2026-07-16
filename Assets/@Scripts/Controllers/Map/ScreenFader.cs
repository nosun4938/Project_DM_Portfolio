using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : InitBase
{
    private Image fadeImage;
    private float fadeDuration = 0.2f;

    private Coroutine _fadeCoroutine;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        fadeImage = GetComponent<Image>();
        return true;
    }

    public void FadeIn()
    {
        PlayFade(1f, 0f);
    }

    public void FadeOut()
    {
        PlayFade(0f, 1f);
    }

    public void PlayFade(float startAlpha, float endAlpha)
    {
        if (_fadeCoroutine != null)
            StopCoroutine( _fadeCoroutine );

        _fadeCoroutine = StartCoroutine(FadeCoroutine(startAlpha, endAlpha));
    }

    private IEnumerator FadeCoroutine(float start, float end)
    {
        float time = 0f;

        Color color = fadeImage.color;
        color.a = start;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeDuration;

            color.a = Mathf.Lerp(start, end, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = end;
        fadeImage.color = color;
    }
}
