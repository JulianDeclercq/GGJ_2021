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
    private GameObject _target;

    public void Spawn()
    {
        
        var randomPos = Random.onUnitSphere * _radius;
        randomPos += transform.position;
        //randomPos.y = Mathf.Abs(randomPos.y);
        //randomPos.z = Random.Range(0, _radius);
        Instantiate(_target, randomPos, transform.rotation, transform);
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
