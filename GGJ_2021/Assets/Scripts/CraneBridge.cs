using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneBridge : MonoBehaviour
{
    [SerializeField]
    [Range(2f, 25f)]
    public float BridgeSpeed = 2f;

    void FixedUpdate()
    {
        float translation = -Input.GetAxis("Vertical") * BridgeSpeed * Time.fixedDeltaTime;
        transform.Translate(translation, 0, 0);
    }
}
