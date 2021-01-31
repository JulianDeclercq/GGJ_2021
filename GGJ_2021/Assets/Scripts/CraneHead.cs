using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneHead : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _grabbers;

    private bool _open = false; //current grabber state

    public float _clawSpeed = 1f;
    private Quaternion _target = Quaternion.identity;
    bool _grabbersBusy = false;
    
    private void ToggleGrabbers()
    {
        if (_grabbersBusy)
            return;

        foreach (var grabber in _grabbers)
            StartCoroutine(RotateGrabber(grabber, _open ? 80f : -80f));
        
        _open = !_open;
    }
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
            ToggleGrabbers();
    }

    private IEnumerator RotateGrabber(GameObject grabber, float yDegrees)
    {
        _grabbersBusy = true;

        // Decide the final rotation of the gameobject
        Quaternion target = grabber.transform.localRotation * Quaternion.Euler(new Vector3(yDegrees, 0, 0));

        Quaternion initialValue = grabber.transform.localRotation;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * _clawSpeed;
            grabber.transform.localRotation = Quaternion.Lerp(initialValue, target, progress);
            yield return new WaitForEndOfFrame();
        }

        _grabbersBusy = false;
    }
}
