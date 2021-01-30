using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField]
    private GameObject _surface;

    private Rigidbody _rigidbody;

    private List<GameObject> _checkedObjects = new List<GameObject>();
    private List<Rigidbody> _floaters = new List<Rigidbody>();

    [SerializeField]
    private float _depthBeforeSubmerged = 1f;

    [SerializeField]
    private float _displacementAmount = 3f;
    void Awake()
    {
        Package p = GetComponent<Package>();
        if (p == null || !p.Properties.HasFlag(PackageProperties.Floats))
        {
            Debug.LogError("Package with floater component does not have Floats property");
            Destroy(this);
        }
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if the floater is above the surface, ignore it
        if (transform.position.y > _surface.transform.position.y)
            return;

        float displacementMultiplier = Mathf.Clamp01((_surface.transform.position.y - transform.position.y) / _depthBeforeSubmerged) * _displacementAmount;

        // WARNING: ADDS GRAVITY SO GRAVITY NEEDS TO BE DISABLED ON THE RIGIDBODY
        _rigidbody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
    }
}
