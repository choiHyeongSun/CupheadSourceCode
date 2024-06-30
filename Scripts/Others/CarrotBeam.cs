using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotBeam : Bullet
{
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
}
