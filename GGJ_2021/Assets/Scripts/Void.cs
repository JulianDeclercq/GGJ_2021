using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Package p = other.GetComponent<Package>();
        if (p != null && p.Winner)
        {
            print("Game Over!");
        }

        Destroy(other.gameObject);
    }
}
