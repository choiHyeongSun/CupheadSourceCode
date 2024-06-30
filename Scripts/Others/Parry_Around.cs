using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry_Around : MonoBehaviour
{
    [SerializeField]
    private float Duration = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(SetActive());
    }

    private IEnumerator SetActive()
    {
        yield return new WaitForSeconds(Duration);
        gameObject.SetActive(false);
    }

    
}
