using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField]
    private GameObject _surface;

    [SerializeField]
    private float _depthBeforeSubmerged = 1f;

    [SerializeField]
    private float _displacementAmount = 3f;

    [SerializeField]
    [Range(0.0f, 2f)]
    private float _waterDrag = 0.99f;

    [SerializeField]
    [Range(0.0f, 2f)]
    private float _waterAngularDrag = 0.5f;

    [SerializeField]
    private List<GameObject> _points;

    private Rigidbody _rigidbody;

    void Awake()
    {
        Package p = GetComponent<Package>();
        if (p == null || !p.Properties.HasFlag(PackageProperties.Floats))
        {
            Debug.LogError("Package with floater component does not have Floats property");
            Destroy(this);
        }
        _rigidbody = GetComponent<Rigidbody>();

        // because gravity is calculated manually in the update, it can be disabled for the rigidbody
        _rigidbody.useGravity = false;
    }

    void Update()
    {
        foreach (var point in _points)
        {
            // gravity calculation
            _rigidbody.AddForceAtPosition(Physics.gravity / _points.Count, point.transform.position, ForceMode.Acceleration);

            // if the floater is above the surface, ignore it
            if (point.transform.position.y > _surface.transform.position.y)
                continue;

            float displacementMultiplier = Mathf.Clamp01((_surface.transform.position.y - point.transform.position.y) / _depthBeforeSubmerged) * _displacementAmount;

            _rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), point.transform.position, ForceMode.Acceleration);
            _rigidbody.AddForce(displacementMultiplier * -_rigidbody.velocity * _waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            _rigidbody.AddTorque(displacementMultiplier * -_rigidbody.angularVelocity * _waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            transform.position = new Vector3(transform.position.x, -10, transform.position.z);
            transform.rotation = Quaternion.Euler(30, transform.rotation.y, transform.rotation.z);
        }
    }
}
