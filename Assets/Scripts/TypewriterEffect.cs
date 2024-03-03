using System.Collections;
using TMPro; // Ensure you're using the TextMeshPro namespace
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    // Speed of typing, in seconds per character
    [SerializeField] private float typingSpeed = 0.05f;
    
    // The full text to display
    [SerializeField] private string fullText;
    
    // Current text displayed
    private string _currentText = ""; 
    
    // Reference to the TextMeshPro component
    [SerializeField] private TMP_Text textComponent; 

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            _currentText = fullText.Substring(0, i);
            textComponent.text = _currentText;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
