using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpot : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("в_Споте");
    }
}
