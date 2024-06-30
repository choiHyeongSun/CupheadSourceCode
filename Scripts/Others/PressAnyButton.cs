using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PressAnyButton : MonoBehaviour
{
    [SerializeField]
    private float SwitchTime;
    [SerializeField]
    private TMP_Text PressFont;
    [SerializeField]
    private Fadeout Fade;
    [SerializeField]
    private GameObject FullScreenObject;

    private bool FadeInCheck = false; 
    private bool IsPress;
    


    private void Start()
    {
        StartCoroutine(SwitchImage());
        Fade.FadeInCallback += () =>
        {
            FadeInCheck = true;
        };


    }

    private void Update()
    {
        if (Input.anyKey == true && IsPress == false && FadeInCheck == true)
        {
            IsPress = true;
            FullScreenObject.SetActive(true);
        }
    }

    private IEnumerator SwitchImage()
    {

        Color color = PressFont.color;

        while (IsPress == false)
        {
            
            color.a = 0.0f;
            PressFont.color = color;
            yield return new WaitForSeconds(SwitchTime);
            color.a = 1.0f;
            PressFont.color = color;
            yield return new WaitForSeconds(SwitchTime);
        }
        color.a = 0.0f;
        PressFont.color = color;
    }






}
