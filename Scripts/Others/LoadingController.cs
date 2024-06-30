using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    private static String CurrentScneeName;

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {

        yield return new WaitForSeconds(1.0f);

        AsyncOperation op = SceneManager.LoadSceneAsync(CurrentScneeName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

    public static String gCurrentSceneName(String value) => CurrentScneeName = value;
}
