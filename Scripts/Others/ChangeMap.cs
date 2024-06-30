using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMap : MonoBehaviour
{

    [SerializeField]
    private String SceneName;
    [SerializeField]
    private GameObject Ui;

    private AActor Actor;
    private int HashNumber;

    private Coroutine FadeCoroutine;


    private void Awake()
    {
        Ui.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AActor>() != null && Actor == null)
        {
            Actor = collision.GetComponent<AActor>();
            Actor.SetEntryChangeMap(true);

            HashNumber = collision.gameObject.GetHashCode();
            Ui.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (HashNumber == collision.gameObject.GetHashCode())
        {
            Ui.SetActive(false);
            Actor.SetEntryChangeMap(false);
            Actor = null;
        }
    }

    private void Update()
    {
        if (Actor != null && FadeCoroutine == null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                LoadingController.gCurrentSceneName(SceneName);

                GameManager game = FindObjectOfType<GameManager>();
                if (game != null)
                {
                    FadeCoroutine = StartCoroutine(game.ScreenFadeOut());
                }
                else
                {
                    Fadeout fade = FindObjectOfType<Fadeout>();
                    if (fade != null)
                    {
                        FadeCoroutine = StartCoroutine(fade.FadeOutCoroutine());
                    }
                }

                Global.IsPreventInputKey = true;
            }
            Ui.transform.rotation = Quaternion.identity;
        }
    }

    private void OnDestroy()
    {
        Global.IsPreventInputKey = false;
    }
}
