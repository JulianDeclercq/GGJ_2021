using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Package p = other.GetComponent<Package>();
        if (p == null)
            return;

        if(p.Winner)
        {
            print("WINWIN");
        }
    }
}
