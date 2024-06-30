using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ttears_Bullet : Bullet
{
    private static readonly string[] TearsDrop = new string[]
    {
        "TearDrop01",
        "TearDrop02",
        "TearDrop03",
        "TearDrop04",
        "TearDrop05",
        "TearDrop06"
    };
    protected override void Update()
    {
        Vector3 boundMin = GetBox2D.bounds.min;
        Vector3 point = Camera.main.WorldToViewportPoint(boundMin);
        if (point.y < 0.1f)
        {
            if (GetPeashotDeath != null)
            {
                Instantiate(GetPeashotDeath, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        int index = Random.Range(0, TearsDrop.Length - 1);
        SoundManager.gInstance.PlaySound(TearsDrop[index], null);
    }



}
