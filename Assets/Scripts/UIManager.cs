using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup aboutMenuCanvasGroup;
    public CanvasGroup optionsMenuCanvasGroup;
    public float fadeDuration;


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
            // Debug.Log($"Fading '{canvasGroup.gameObject.name}' to alpha: {newAlpha} at time: {Time.time}");

            canvasGroup.alpha = newAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        // Debug.Log($"Fade completed on {canvasGroup.gameObject.name}. Final Alpha: {endAlpha}. Time: {Time.time}");

        if (isFadingIn)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.gameObject.SetActive(false);
            // Debug.Log($"'{canvasGroup.gameObject.name}' set to inactive after fade out. Time: {Time.time}");
        }

        onComplete?.Invoke();
    }
}