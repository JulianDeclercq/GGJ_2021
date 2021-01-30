using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneHead : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _grabbers;

    private bool _open = false; //current grabber state
    
    private void ToggleGrabbers()
    {
        SetGrabberYRotation(_open ? 80f : -80f);
        _open = !_open;
    }
    private void SetGrabberYRotation(float yDegrees)
    {
        foreach (var grabber in _grabbers)
            grabber.transform.Rotate(new Vector3(yDegrees, 0, 0));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleGrabbers();
        }
    }
}
