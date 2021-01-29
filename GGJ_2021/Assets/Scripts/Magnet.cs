using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private float _radius = 5f;

    [SerializeField]
    private float _forceFactor = 1f;

    private int _layerMask = 0;

    void Start()
    {
        _layerMask = ~LayerMask.GetMask("Ignore Everything");
    }

    void Update()
    {
        // TODO: Check if magnetic through layermask
        var inRadius = Physics.OverlapSphere(transform.position, _radius, _layerMask);
        foreach(var collider in inRadius)
        {
            var rb = collider.GetComponent<Rigidbody>();
            if (rb == null)
                continue;

            rb.AddForce((transform.position - rb.transform.position) * _forceFactor);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
