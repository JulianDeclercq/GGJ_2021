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
    private bool _floats = false;

    void Awake()
    {
        Package p = GetComponent<Package>();
        _floats = p.Properties.HasFlag(PackageProperties.Floats);
        _rigidbody = GetComponent<Rigidbody>();

        // because gravity is calculated manually in the update, it can be disabled for the rigidbody
        if (_floats) 
            _rigidbody.useGravity = false;
    }

    void Update()
    {
        foreach (var point in _points)
        {
            // gravity calculation, let rigidbody take care of non floaters
            if (_floats)
            {
                _rigidbody.AddForceAtPosition(Physics.gravity / _points.Count, point.transform.position, ForceMode.Acceleration);
            }

            // if the point is above the surface, ignore it
            if (point.transform.position.y > _surface.transform.position.y)
                continue;

            float displacementMultiplier = Mathf.Clamp01((_surface.transform.position.y - point.transform.position.y) / _depthBeforeSubmerged) * _displacementAmount;

            // floating
            if (_floats)
            {
                _rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), point.transform.position, ForceMode.Acceleration);
            }

            //_rigidbody.AddForce(displacementMultiplier * -_rigidbody.velocity * _waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //_rigidbody.AddTorque(displacementMultiplier * -_rigidbody.angularVelocity * _waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            _rigidbody.AddForce(displacementMultiplier * -_rigidbody.velocity * (_waterDrag / _rigidbody.mass) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            _rigidbody.AddTorque(displacementMultiplier * -_rigidbody.angularVelocity * (_waterAngularDrag / _rigidbody.mass) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        // manual reset
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
            transform.rotation = Quaternion.Euler(30, transform.rotation.y, transform.rotation.z);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
            transform.rotation = Quaternion.Euler(30, transform.rotation.y, transform.rotation.z);
        }
#endif
    }
}
