using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialParryGroup : MonoBehaviour
{
 

    [SerializeField]
    private Sprite Normal;

    [SerializeField]
    private Sprite Parry;

    private int Index = 0;
    private List<SpriteRenderer> Renderes2D = new List<SpriteRenderer>();
    private TutorialParry[] Parries;

    private void Awake()
    {
        
        Parries = GetComponentsInChildren<TutorialParry>();

        foreach (var parry in Parries)
        {
            Renderes2D.Add(parry.gameObject.GetComponent<SpriteRenderer>());
        }

        foreach (var render in Renderes2D)
        {
            render.sprite = Normal;
        }

        foreach (var parry in Parries)
        {
            parry.Callback = CallbackFunction;
        }

        Renderes2D[Index].sprite = Parry;
        Parries[Index].gParry = true;
    }

    private void CallbackFunction(Collider2D collision)
    {
        Renderes2D[Index].sprite = Normal;
        Parries[Index].gParry = false;
        Index = (Index + 1) % Renderes2D.Count;
        Renderes2D[Index].sprite = Parry;
        Parries[Index].gParry = true;
    }



}
