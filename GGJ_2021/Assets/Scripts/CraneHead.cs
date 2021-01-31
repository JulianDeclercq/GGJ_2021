using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneHead : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _grabbers;

    private bool _open = false; //current grabber state

    public float _rotationSpeed = 1f;
    private Quaternion _target = Quaternion.identity;
    bool _grabbersBusy = false;
    
    private void ToggleGrabbers()
    {
        if (_grabbersBusy)
            return;

        SetGrabberYRotation(_open ? 80f : -80f);
        _open = !_open;
    }
    private void SetGrabberYRotation(float yDegrees)
    {
        foreach (var grabber in _grabbers)
        {
            StartCoroutine(RotateGrabber(grabber, yDegrees));

            continue;
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
        var step = _rotationSpeed * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        _grabbers[0].transform.localRotation = Quaternion.RotateTowards(transform.rotation, _target, step);
    }

    private IEnumerator RotateGrabber(GameObject grabber, float yDegrees)
    {
        _grabbersBusy = true;
        // Shrink the object completely
        //grow.transform.localScale = Vector3.zero;

        // Decide the final scale of the gameobject
        Quaternion target = grabber.transform.localRotation * Quaternion.Euler(new Vector3(yDegrees, 0, 0));

        // Time in seconds to complete the growth process
        //int time = Random.Range(1, _treeGrowthSpeed);
        Quaternion initialValue = grabber.transform.localRotation;
        float progress = 0f;
        //float rate = 1f / time;
        float rate = _rotationSpeed;

        // Gradually grow
        while (progress < 1f)
        {
            progress += Time.deltaTime * rate;
            grabber.transform.localRotation = Quaternion.Lerp(initialValue, target, progress);
            yield return new WaitForEndOfFrame();
        }

        _grabbersBusy = false;
    }
}
