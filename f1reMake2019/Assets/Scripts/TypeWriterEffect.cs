using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TypeWriterEffect : MonoBehaviour
{

    // a fully functioning typeWriter system. it can do typeWriting with TMP's, and it can be altered to make typeWriting with Text components.

    [SerializeField] private float typeWriterSpeed = 50f;

    public Coroutine Run(string textToType, TMP_Text textLabel, float WaitingTime, float typeWriterSpeed)
    {
        return StartCoroutine(typeText(textToType, textLabel, WaitingTime, typeWriterSpeed));
    }

    private IEnumerator typeText(string textToType, TMP_Text textLabel, float WaitingTime, float typeWriterSpeed)
    {
       // Debug.Log("Starting to type text lmao");
        textLabel.text = string.Empty;

        

        yield return new WaitForSeconds(WaitingTime);

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t); 

            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);
            yield return null;
            //Debug.Log("Struggling in a loop");
        }
                                 
        textLabel.text = textToType;
        //Debug.Log("The effort was all worth it.");

        yield return null;
    }
}
