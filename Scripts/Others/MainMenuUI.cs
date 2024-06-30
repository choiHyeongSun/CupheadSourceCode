using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField]
    private GameObject PressAnyCanvas;
    [SerializeField]
    private GameObject MainmenuGroup;
    [SerializeField]
    private float FullScreenFadeTime;
    


    private Image FullScreenImage;

    private void Awake()
    {
        FullScreenImage = transform.Find("Fade").GetComponent<Image>();

    }

    private void OnEnable()
    {
        StartCoroutine(FullScreenFadeInOut());
    }

    private IEnumerator FullScreenFadeInOut()
    {

        Color color = FullScreenImage.color;
        float time = Time.time + FullScreenFadeTime;
        while (time > Time.time)
        {

            color.a = 1.0f - ((time - Time.time)) / FullScreenFadeTime;
            FullScreenImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        color.a = 1.0f;
        FullScreenImage.color = color;
        PressAnyCanvas.SetActive(false);
        MainmenuGroup.SetActive(true);

        time = Time.time + FullScreenFadeTime;
        while (time > Time.time)
        {

            color.a = ((time - Time.time)) / FullScreenFadeTime;
            FullScreenImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        color.a = 0.0f;
        FullScreenImage.color = color;

    }

}
