using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections;

public class AnimationsManager : MonoBehaviour
{
    public UIManager uiManager; 
    public VideoPlayer videoPlayer; 
    public CanvasGroup mainMenuGroup;
    public CanvasGroup aboutMenuGroup;
    public CanvasGroup optionsMenuGroup;
    public VideoClip singlePageAnimation;
    public VideoClip twoPageAnimation;
    public VideoClip page1Reversed;
    public VideoClip page2Reversed;


    public void PlayVideo(VideoClip clipToPlay, Action onVideoFinished)
    {
        videoPlayer.clip = clipToPlay;
        videoPlayer.prepareCompleted += (source) =>
        {
            source.Play();
            StartCoroutine(WaitForVideoEnd(onVideoFinished));
        };
        videoPlayer.Prepare();
    }


    private IEnumerator WaitForVideoEnd(Action onVideoFinished)
    {   
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        Debug.Log("Video finished playing.");
        onVideoFinished?.Invoke();
    }  

    
    public void OnAboutClicked()
    {
        StartTransition(singlePageAnimation, mainMenuGroup, aboutMenuGroup);
    }

    public void OnOptionsClicked()
    {
        StartTransition(twoPageAnimation, mainMenuGroup, optionsMenuGroup);
    }

    public void OnBackFromAboutClicked()
    {
        StartTransition(page1Reversed, aboutMenuGroup, mainMenuGroup);
    }

    public void OnBackFromOptionsClicked()
    {
        StartTransition(page2Reversed, optionsMenuGroup, mainMenuGroup);
    }

    
private void StartTransition(VideoClip videoClip, CanvasGroup initialFadeOutGroup, CanvasGroup initialFadeInGroup)
{
    CanvasGroup fadeOutGroup = initialFadeOutGroup;
    CanvasGroup fadeInGroup = initialFadeInGroup;

    uiManager.Fade(fadeOutGroup, false, () =>
    {
        fadeOutGroup.gameObject.SetActive(false); 

        PlayVideo(videoClip, () =>
        {
            fadeInGroup.gameObject.SetActive(true); 
            fadeInGroup.alpha = 0; 
            Debug.Log($"Before fading in, fadeOutGroup is {fadeOutGroup.gameObject.name} and fadeInGroup is {fadeInGroup.gameObject.name}");
            uiManager.Fade(fadeInGroup, true, () =>
            {
                fadeInGroup = null;
                fadeOutGroup = null;
            });
        });
    });
}


}
