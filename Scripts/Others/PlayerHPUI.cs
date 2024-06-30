using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHPUI : MonoBehaviour
{
    [SerializeField]
    private APlayer Player;
    [SerializeField]
    private Sprite[] Sprites;
    [SerializeField]
    private Image HpImage;

    private StatusComponent Status;

    private void Awake()
    {
        if (Player != null)
            Status = Player.GetComponent<StatusComponent>();
    }


    private void Update()
    {
        int currentHP = Mathf.RoundToInt(Status.GetCurrentHp);
        HpImage.sprite = Sprites[currentHP];
    }

}
