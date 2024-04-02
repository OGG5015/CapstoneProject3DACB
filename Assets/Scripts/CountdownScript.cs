using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CountdownScript : MonoBehaviour
{
    public GameObject bubble;
    public TMP_Text number;
    public float trueTime = 3.0f;
    //public float fadeSpeed = 1.0f;
    //public CanvasGroup bobble;
    
    void Update()
    {
        trueTime = trueTime - Time.deltaTime;
        if (trueTime > 0)
        {
            //StartCoroutine(FadeInBubble());
            number.text = (Mathf.FloorToInt(trueTime)).ToString();
            

            // fade and random pos effect
            //StartCoroutine(FadeOutBubble());
           
        }
        else
        {
            number.text = "Let's go!";
            SceneManager.LoadScene("Game View");
        }
    }

    //public IEnumerator fadeinbubble()
    //{
    //    yield return null;
    //}

    //public IEnumerator FadeOutBubble()
    //{
    //    while (bobble.GetComponent<CanvasGroup>().alpha > 0)
    //    {

    //        float fadeamount = bobble.alpha + (fadeSpeed * Time.deltaTime);

    //        bobble.alpha = fadeamount;
    //        yield return null;
    //    }
    //}
}
