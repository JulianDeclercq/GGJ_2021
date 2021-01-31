using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [SerializeField]
    private int _count = 10;

    [SerializeField]
    private float _radius = 10;

    [SerializeField]
    private List<GameObject> _targets;

    bool _first = true; // make sure 1 target exists

    public void Spawn()
    {
        var randomPos = Random.insideUnitSphere * _radius;
        randomPos += transform.position;

        // force spawn a correct package
        if (_first)
        {
            Instantiate(_targets[4], randomPos, transform.rotation, transform);
            _first = false;
            return;
        }

        Instantiate(_targets[Random.Range(0, _targets.Count)], randomPos, transform.rotation, transform);
    }

    void Start()
    {
        for (int i = 0; i < _count; ++i)
            Spawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
