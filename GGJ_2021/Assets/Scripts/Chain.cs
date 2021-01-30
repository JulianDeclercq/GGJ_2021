using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField]
    private GameObject _head;

    [SerializeField]
    private GameObject _base;

    [SerializeField]
    private CraneBridge _bridge;

    [SerializeField]
    private float _headDownForce = 30f;

    [SerializeField]
    private List<ChainLink> _links;

    private Rigidbody _headRb;

    private ChainLink _masterLink;
    
    void Start()
    {
        _headRb = _head.GetComponent<Rigidbody>();
        _masterLink = _links[0];
    }

    void FixedUpdate()
    {
        _headRb.AddForce(Vector3.down * _headDownForce, ForceMode.Acceleration);

        // crane base
        float hTranslation = Input.GetAxis("Horizontal") * _bridge.BridgeSpeed * Time.fixedDeltaTime;
        _base.transform.position += new Vector3(0, 0, hTranslation);

        foreach (var link in _links)
        {
            if (link.Rigidbody.isKinematic)
            {
                float vTranslation = -Input.GetAxis("Vertical") * _bridge.BridgeSpeed * Time.fixedDeltaTime;
                //float hTranslation = Input.GetAxis("Horizontal") * _bridge.BridgeSpeed * Time.fixedDeltaTime;
                link.transform.position += new Vector3(vTranslation, 0, hTranslation);
            }
        }

        // locking link (going up)
        bool masterIsLowest = _masterLink.Index >= _links.Count - 1;
        if (!masterIsLowest) // if masterlink is not the last existing link
        {
            var nextLink = _links[_masterLink.Index + 1]; // under master link
            if (nextLink.transform.position.y > _base.transform.position.y)
            {
                nextLink.Rigidbody.isKinematic = true;
                _masterLink = nextLink;
            }
        }

        // releasing link (going down)
        if (_masterLink.Index > 0) // never release the very top link
        {
            if (_masterLink.transform.position.y < _base.transform.position.y)
            {
                _masterLink.Rigidbody.isKinematic = false;

                _masterLink = _links[_masterLink.Index - 1];
            }
        }
    }

    private void TranslateKineticLinks(Vector3 translation)
    {
        
    }
}
