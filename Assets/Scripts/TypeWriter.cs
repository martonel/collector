using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    public TMP_Text textComponent; // A sz�vegmez�, amiben megjelenik az �r�s
    public Button nextButton;  // A gomb, amit letiltunk az �r�s k�zben
    public float typeSpeed = 0.05f; // Az �r�s sebess�ge (mp bet�nk�nt)
    public string startText;

    private void Start()
    {
        // Teszt sz�veg automatikus elind�t�sq
        StartTypeWriter(startText);
    }

    public void StartTypeWriter(string textMessage)
    {
        StopAllCoroutines(); // Ha �ppen fut egy �r�s, �ll�tsuk meg
        StartCoroutine(TypeWriterEffect(textMessage));
    }

    private IEnumerator TypeWriterEffect(string textMessage)
    {
        if (nextButton != null)
            nextButton.interactable = false; // Gomb inaktiv�l�sa

        textComponent.text = ""; // T�r�lj�k a sz�vegmez�t
        foreach (char letter in textMessage.ToCharArray())
        {
            textComponent.text += letter; // Bet� hozz�ad�sa
            yield return new WaitForSeconds(typeSpeed); // V�rakoz�s a k�vetkez� bet�re
        }

        if (nextButton != null)
            nextButton.interactable = true; // Gomb �jra aktiv�l�sa
    }
}