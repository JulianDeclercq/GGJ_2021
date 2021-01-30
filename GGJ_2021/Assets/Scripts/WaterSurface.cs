using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    private List<GameObject> _checkedObjects = new List<GameObject>();
    private List<Rigidbody> _floaters = new List<Rigidbody>();

    [SerializeField]
    private float _depthBeforeSubmerged = 1f;

    [SerializeField]
    private float _displacementAmount = 3f;

    private void OnTriggerEnter(Collider other)
    {
        // ignore duplicates
        if (_checkedObjects.Contains(other.gameObject))
            return;

        Package p = other.gameObject.GetComponent<Package>();
        if (p != null && p.Properties.HasFlag(PackageProperties.Floats))
        {
            _floaters.Add(other.gameObject.GetComponent<Rigidbody>());
            print($"Floater found in water {other.gameObject.name}");
        }
            
        _checkedObjects.Add(other.gameObject);
    }

    private void Update()
    {
        foreach(var floater in _floaters)
        {
            // if the floater is above the surface, ignore it
            if (floater.transform.position.y > transform.position.y)
                continue;

            float displacementMultiplier = Mathf.Clamp01((transform.position.y - floater.transform.position.y) / _depthBeforeSubmerged) * _displacementAmount;

            // WARNING: ADDS GRAVITY SO GRAVITY NEEDS TO BE DISABLED ON THE RIGIDBODY
            floater.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, Vector3.one);
        foreach (var floater in _floaters)
        {
            
            // if the floater is above the surface, ignore it
            if (floater.transform.position.y > transform.position.y)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(floater.transform.position, 1f);
                continue;
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(floater.transform.position, 1f);
            }
        }
    }
}

