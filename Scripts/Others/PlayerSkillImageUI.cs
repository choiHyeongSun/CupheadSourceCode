using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillImageUI : MonoBehaviour
{
    private APlayer Player;
    private Image[] ChildImages;

    private void Awake()
    {
        ChildImages = GetComponentsInChildren<Image>();
        Player = FindObjectOfType<APlayer>();
    }

    private void Update()
    {
        float temp = Player.gSkillGauge;
        bool check = false;
        for (int i = 0; i < ChildImages.Length; i++)
        {
            if (check == true)
            {
                ChildImages[i].fillAmount = 0.0f;
                continue;
            }

            if (temp >= 1.0f)
            {
                ChildImages[i].fillAmount = 1.0f;
                temp -= 1.0f;
                continue;
            }
            ChildImages[i].fillAmount = temp;
            check = true;
        }
    }

}
