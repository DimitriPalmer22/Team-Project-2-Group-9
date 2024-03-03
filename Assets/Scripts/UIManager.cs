using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup aboutMenuCanvasGroup;
    [SerializeField] private CanvasGroup optionsMenuCanvasGroup;
    [SerializeField] private float fadeDuration;


    public void Fade(CanvasGroup canvasGroup, bool fadeIn, Action onComplete = null)
    {
        Debug.Log($"Starting Fade on {canvasGroup.gameObject.name}. FadeIn: {fadeIn}. Time: {Time.time}");
        StartCoroutine(FadeCanvasGroup(canvasGroup, fadeIn ? 0f : 1f, fadeIn ? 1f : 0f, onComplete));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, Action onComplete)
    {
        float elapsedTime = 0f;
        bool isFadingIn = startAlpha < endAlpha;

        if (isFadingIn)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            canvasGroup.alpha = newAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (isFadingIn)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.gameObject.SetActive(false);
        }

        onComplete?.Invoke();
    }
}