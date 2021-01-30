using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneHead : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _grabbers;

    private bool _open = false; //current grabber state

    public float speed = 1f;
    private Quaternion _target = Quaternion.identity;
    
    private void ToggleGrabbers()
    {
        SetGrabberYRotation(_open ? 80f : -80f);
        _open = !_open;
    }
    private void SetGrabberYRotation(float yDegrees)
    {
        foreach (var grabber in _grabbers)
        {
            grabber.transform.localRotation *= Quaternion.Euler(new Vector3(yDegrees, 0, 0));

            continue;
            if(_target == Quaternion.identity)
                _target = grabber.transform.localRotation * Quaternion.Euler(new Vector3(yDegrees, 0, 0));
        }
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            ToggleGrabbers();
        }

        if (_target == Quaternion.identity)
            return;

        // The step size is equal to speed times frame time.
        var step = speed * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        _grabbers[0].transform.localRotation = Quaternion.RotateTowards(transform.rotation, _target, step);
    }
}
