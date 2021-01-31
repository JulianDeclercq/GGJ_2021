using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneHead : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _grabbers;

    [SerializeField]
    private Magnet _magnet;

    [SerializeField]
    private float _clawSpeed = 1f;

    [SerializeField]
    private float _angleLimit = 80f;

    [SerializeField]
    private float _pushingForce = 1f;

    private Rigidbody _rb;

    private bool _open = false; //current grabber state

    private Quaternion _target = Quaternion.identity;
    bool _grabbersBusy = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        ToggleGrabbers();
    }
    void Update()
    {
        if (GameManager.Instance.InputEnabled && Input.GetButtonUp("Fire1"))
            ToggleGrabbers();

        if (!GameManager.Instance.OnCooldown && GameManager.Instance.InputEnabled && (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Space)))
        {
            _rb.AddRelativeForce(-Vector3.forward * _pushingForce, ForceMode.Impulse);
            GameManager.Instance.ResetPushCooldown();
        }
    }

    private void ToggleGrabbers()
    {
        if (_grabbersBusy)
            return;

        foreach (var grabber in _grabbers)
            StartCoroutine(RotateGrabber(grabber, _open ? _angleLimit : -_angleLimit));
        
        _open = !_open;
    }

    private IEnumerator RotateGrabber(GameObject grabber, float yDegrees)
    {
        _grabbersBusy = true;

        bool closing = _open;
        _magnet.enabled = closing;

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
