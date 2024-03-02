using System.Collections;
using TMPro; // Ensure you're using the TextMeshPro namespace
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f; // Speed of typing, in seconds per character
    public string fullText; // The full text to display
    private string currentText = ""; // Current text displayed
    public TMP_Text textComponent; // Reference to the TextMeshPro component

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
