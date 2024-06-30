using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fadeout : MonoBehaviour
{
    [SerializeField]
    private Material FullScreenMat;
    [SerializeField]
    private float FadeOutTime = 0.5f;
    [SerializeField] 
    private float FadeOutDefault = 2.2f;

    public Action FadeInCallback;
    public Action FadeOutCallback;

    private void Start()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        if (FullScreenMat == null) yield break;

        float scale = FadeOutDefault;
        float time = Time.time + FadeOutTime;
        while (time > Time.time)
        {
            FullScreenMat.SetFloat("_Scale", scale * (1.0f - ((time - Time.time)) / FadeOutTime));
            yield return new WaitForEndOfFrame();
        }
        FullScreenMat.SetFloat("_Scale", scale);
        if (FadeInCallback != null)
        {
            FadeInCallback();
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
        if (FullScreenMat == null) yield break;

        float scale = FadeOutDefault;
        float time = Time.time + FadeOutTime;
        while (time > Time.time)
        {
            FullScreenMat.SetFloat("_Scale", scale * ((time - Time.time) / FadeOutTime));
            yield return new WaitForEndOfFrame();
        }
        FullScreenMat.SetFloat("_Scale", 0.0f);

        if (FadeInCallback != null)
        {
            FadeInCallback();
        }
        SceneManager.LoadScene("LoadingScene");


    }
}
