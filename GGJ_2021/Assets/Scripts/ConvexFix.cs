using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexFix : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    void Update()
    {
        transform.localPosition = _target.localPosition;
        transform.localRotation = _target.localRotation;
        transform.localScale = _target.localScale;
    }
}
