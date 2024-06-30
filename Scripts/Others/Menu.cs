using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] 
    private Fadeout FadeOut;
    [SerializeField]
    private TMP_Text[] Menus;
    private int SelectIndex = 0;

    private bool IsKeyboardLock = false;

    internal enum EMenuType
    {
        START = 0,
        QUIT,
        MAX,
    }


    private void Update()
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            if (i == SelectIndex)
            {
                Menus[i].color = Color.yellow;
            }
            else
            {
                Menus[i].color = Color.white;
            }
        }

        if (IsKeyboardLock == true) return;
        if (Input.GetKeyDown(KeyCode.Return) == true || Input.GetKeyDown(KeyCode.Z) == true)
        {
            switch (SelectIndex)
            {
                case (int)EMenuType.START:
                    LoadingController.gCurrentSceneName("TutorialScene");
                    StartCoroutine(FadeOut.FadeOutCoroutine());
                    break;
                case (int)EMenuType.QUIT:
                    Application.Quit();
                    break;

            }

            IsKeyboardLock = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            SelectIndex++;
            SelectIndex = SelectIndex % (int)EMenuType.MAX;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            SelectIndex--;
            SelectIndex = SelectIndex < 0 ? (int)EMenuType.MAX - 1 : SelectIndex;
        }
    }

    public bool gSetKeybardLock
    {
        get => IsKeyboardLock;
        set => IsKeyboardLock = value;
    }

    
    

}
