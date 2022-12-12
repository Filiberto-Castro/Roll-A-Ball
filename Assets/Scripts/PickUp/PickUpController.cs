using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject[] puntosDePickUp;
    public int score;

    void Start()
    {
        GameObject puntosPickUpObjeto = GameObject.Find("PickUps");
        int childCount = puntosPickUpObjeto.transform.childCount;
        puntosDePickUp = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            puntosDePickUp[i] = puntosPickUpObjeto.transform.GetChild(i).gameObject;
        }
        score = childCount;
    }

    void Update()
    {

    }
}
